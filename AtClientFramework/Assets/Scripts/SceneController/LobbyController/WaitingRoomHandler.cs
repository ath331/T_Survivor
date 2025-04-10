using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo
{
    public PlayerInfo()
    {
        objectInfo = new ObjectInfo();
    }

    public ObjectInfo objectInfo;

    public PlayerController playerController;
}

public class WaitingRoomHandler : MonoBehaviour
{
    [SerializeField] private LobbyController lobbyController;

    [SerializeField] private TMP_Text titleText;

    [SerializeField] private Transform[] spawnTransform;

    [SerializeField] private Button exitButton;

    // 생성된 캐릭터 인스턴스를 보관하는 리스트
    private List<PlayerInfo> playerInfos;

    // 각 스폰 슬롯의 사용 여부를 관리하는 배열
    private bool[] spawnSlotsOccupied;

    // 방장 여부
    bool isRoomLeader = false;

    ERoomState roomState = ERoomState.RoomStateNone;

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;

    private string titleName = "";

    void OnEnable()
    {
        WaitRoomOutNotify_Strategy.OnRoomOutNotify += NotifyRoomOutPlayer;

        exitButton.onClick.AddListener(OnClickExit);
    }

    void OnDisable()
    {
        WaitRoomOutNotify_Strategy.OnRoomOutNotify -= NotifyRoomOutPlayer;

        exitButton.onClick.RemoveAllListeners();

        Destroy_AllCharacter();

        isRoomLeader = false;
    }

    public void IsOnRoomLeader()
    {
        isRoomLeader = true;
    }

    public void NotifyRoomOutPlayer(S_WaitingRoomOutNotify message)
    {
        // TODO : 룸 리더가 나갔을 경우, 아닌 경우
        
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].objectInfo.Id == message.Player.Id)
            {
                Destroy_CharacterAtSlot(i);
            }
        }
    }

    public void NotifyEnterPlayer(S_WaitingRoomEnterNotify message)
    {
        SpawnOtherCharacter(message);
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

        SpawnMyCharacter();
    }

    private void SetRoomInfo(RoomInfo roomInfo)
    {
        titleName = roomInfo.Name;

        roomNumber = roomInfo.Num;

        roomState = roomInfo.RoomState;

        cur_count = roomInfo.CurCount;

        max_count = roomInfo.MaxCount;
    }

    public void OnClickReady()
    {

    }

    private void OnClickExit()
    {
        C_WaitingRoomOut roomOut = new C_WaitingRoomOut();

        NetworkManager.Instance.Send(roomOut);

        lobbyController.SetEnableControl(isLobby: true, isWaitRoom: false);
    }

    private void SpawnCharacter(ulong playerId)
    {
        for (int i = 0; i < max_count; i++)
        {
            if (!spawnSlotsOccupied[i])
            {
                PlayerController controller = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);
                if (controller == null)
                {
                    Debug.LogError("PlayerController 풀링 실패");
                    return;
                }
                controller.enabled = false;
                controller.rb.useGravity = false;

                playerInfos[i].playerController = controller;
                playerInfos[i].objectInfo.Id = playerId;
                spawnSlotsOccupied[i] = true;
                break;
            }
        }
    }

    // MyCharacter 호출
    private void SpawnMyCharacter()
    {
        SpawnCharacter(MercuryHelper.mercuryId);
    }

    // OtherCharacter 호출 (메시지에서 가져온 Id 사용)
    public void SpawnOtherCharacter(S_WaitingRoomEnterNotify message)
    {
        SpawnCharacter(message.Player.Id);
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
                playerInfos[slotIndex] = new PlayerInfo();
            }
        }
    }

    public void Destroy_AllCharacter()
    {
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i] != null && playerInfos[i].playerController != null)
            {
                ReturnCharacter(playerInfos[i].playerController);

                playerInfos[i] = new PlayerInfo();
                spawnSlotsOccupied[i] = false;
            }
        }
    }

    public void ReturnCharacter(PlayerController character)
    {
        character.enabled = true;
        character.rb.useGravity = true;

        ObjectPoolManager.Instance.Return(character.gameObject);
    }
}
