using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInfo()
    {
        objectInfo = new ObjectInfo();
    }

    public ObjectInfo objectInfo;

    public PlayerController playerController;
}

public class GameRoomHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private Transform[] spawnTransform;

    // 생성된 캐릭터 인스턴스를 보관하는 리스트
    private List<PlayerInfo> playerInfos;

    // 각 스폰 슬롯의 사용 여부를 관리하는 배열
    private bool[] spawnSlotsOccupied;

    ERoomState roomState = ERoomState.RoomStateNone;

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;

    private string titleName = "";

    void OnDisable()
    {
        Destroy_All_Chracter();
    }

    public void NotifyPlayer(S_WaitingRoomEnterNotify message)
    {
        Spawn_Other_Character(message);
    }

    public void SetMaKeRoom(RoomInfo roomInfo)
    {
        SetRoomInfo(roomInfo);

        titleText.text = $"[{roomNumber}] {titleName}";

        playerInfos = new List<PlayerInfo>(max_count);

        for (int i = 0; i < max_count; i++)
        {
            playerInfos.Add(new PlayerInfo());  // 각 인덱스에 PlayerInfo 객체 추가
        }

        spawnSlotsOccupied = new bool[max_count];

        Spawn_My_Character();
    }

    private void SetRoomInfo(RoomInfo roomInfo)
    {
        titleName = roomInfo.Name;

        roomNumber = roomInfo.Num;

        roomState = roomInfo.RoomState;

        cur_count = roomInfo.CurCount;

        max_count = roomInfo.MaxCount;
    }

    public void Spawn_My_Character()
    {
        for (int i = 0; i < max_count; i++)
        {
            if (!spawnSlotsOccupied[i])
            {
                PlayerController myPlayerController = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);

                myPlayerController.GetComponent<PlayerController>().enabled = false;
                myPlayerController.rb.useGravity = false;

                playerInfos[i].playerController = myPlayerController;

                playerInfos[i].objectInfo.Id = MercuryHelper.mercuryId;

                spawnSlotsOccupied[i] = true;

                break;
            }
        }
    }

    /// <summary>
    /// TODO : 다른 사람의 캐릭터 정보 필요.
    /// </summary>
    public void Spawn_Other_Character(S_WaitingRoomEnterNotify message)
    {
        for (int i = 0; i < max_count; i++)
        {
            if (!spawnSlotsOccupied[i])
            {
                PlayerController otherPlayerController = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);

                otherPlayerController.GetComponent<PlayerController>().enabled = false;
                otherPlayerController.rb.useGravity = false;

                playerInfos[i].playerController = otherPlayerController;
                playerInfos[i].objectInfo.Id = message.Player.Id;

                spawnSlotsOccupied[i] = true;

                break;
            }
        }
    }

    public void Destroy_CharacterAtSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < spawnTransform.Length)
        {
            PlayerController character = playerInfos[slotIndex].playerController;
            if (character != null)
            {
                ReturnCharacter(character);
                spawnSlotsOccupied[slotIndex] = false;
                playerInfos[slotIndex].playerController = null; // 할당된 캐릭터 제거
                playerInfos[slotIndex].objectInfo = new ObjectInfo();
            }
        }
    }

    public void Destroy_All_Chracter()
    {
        foreach (var character in playerInfos)
        {
            ReturnCharacter(character.playerController);
        }
    }

    public void ReturnCharacter(PlayerController character)
    {
        character.enabled = true;
        character.rb.useGravity = true;

        ObjectPoolManager.Instance.Return(character.gameObject);
    }
}
