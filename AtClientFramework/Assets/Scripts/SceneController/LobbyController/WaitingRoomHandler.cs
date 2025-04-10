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

    // ������ ĳ���� �ν��Ͻ��� �����ϴ� ����Ʈ
    private List<PlayerInfo> playerInfos;

    // �� ���� ������ ��� ���θ� �����ϴ� �迭
    private bool[] spawnSlotsOccupied;

    // ���� ����
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
        // TODO : �� ������ ������ ���, �ƴ� ���
        
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
            playerInfos.Add(new PlayerInfo());  // �� �ε����� PlayerInfo ��ü �߰�
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
                    Debug.LogError("PlayerController Ǯ�� ����");
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

    // MyCharacter ȣ��
    private void SpawnMyCharacter()
    {
        SpawnCharacter(MercuryHelper.mercuryId);
    }

    // OtherCharacter ȣ�� (�޽������� ������ Id ���)
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
