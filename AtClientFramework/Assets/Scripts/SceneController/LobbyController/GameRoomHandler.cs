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

    // ������ ĳ���� �ν��Ͻ��� �����ϴ� ����Ʈ
    private List<PlayerInfo> playerInfos;

    // �� ���� ������ ��� ���θ� �����ϴ� �迭
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
            playerInfos.Add(new PlayerInfo());  // �� �ε����� PlayerInfo ��ü �߰�
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
    /// TODO : �ٸ� ����� ĳ���� ���� �ʿ�.
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
                playerInfos[slotIndex].playerController = null; // �Ҵ�� ĳ���� ����
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
