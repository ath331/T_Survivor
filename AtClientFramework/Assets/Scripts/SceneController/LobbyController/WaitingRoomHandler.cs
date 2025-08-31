using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Handler;
using Cysharp.Threading.Tasks;
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

    public bool isReady;
}

public class WaitingRoomHandler : MonoBehaviour
{
    [SerializeField] private LobbyController lobbyController;

    [SerializeField] private TMP_Text titleText;

    [SerializeField] private Transform[] spawnTransform;

    [SerializeField] private Button exitButton;

    [SerializeField] private Button readyButton;

    [SerializeField] private Button gameStartButton;

    [SerializeField] private TMP_Text readyButtonText;

    [SerializeField] private GameObject[] readyImages;

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

    private bool isExit = false;

    private int spawnIndex = 0;

    private bool isReady = false; // 내 상태

    private bool isAllReady = false;

    void OnEnable()
    {
        // C_WaitingRoomOut 을 보내면 S_WaitingRoomOutNotify이 다른 클라한테 가고  S_WaitingRoomOut 는 본인한테 옴
        PacketEventManager.Subscribe<S_WaitingRoomOutNotify>(NotifyRoomOutPlayer);
        PacketEventManager.Subscribe<S_WaitingRoomOut>(Receive_WaitRoomOut);

        PacketEventManager.Subscribe<S_ChangeWaitingState>(OnReceiveChangeWaitingState);
        PacketEventManager.Subscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeWaitingStateNotify);

        exitButton.onClick.AddListener(OnClickExit);
        readyButton.onClick.AddListener(OnClickReady);
        gameStartButton.onClick.AddListener(OnClickStart);

        isExit = false;
    }

    void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_WaitingRoomOutNotify>(NotifyRoomOutPlayer);
        PacketEventManager.Unsubscribe<S_WaitingRoomOut>(Receive_WaitRoomOut);

        PacketEventManager.Unsubscribe<S_ChangeWaitingState>(OnReceiveChangeWaitingState);
        PacketEventManager.Unsubscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeWaitingStateNotify);

        Destroy_AllCharacter();

        isRoomLeader = false;

        exitButton.onClick.RemoveAllListeners();

        readyButton.onClick.RemoveAllListeners();

        gameStartButton.onClick.RemoveAllListeners();
    }

    public void IsOnRoomLeader()
    {
        isRoomLeader = true;

        gameStartButton.gameObject.SetActive(true);
        gameStartButton.interactable = false;
    }

    public void NotifyRoomOutPlayer(S_WaitingRoomOutNotify message)
    {
        // TODO : 룸 리더가 나갔을 경우, 아닌 경우
        
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].objectInfo.Id == message.Player.Id)
            {
                Destroy_CharacterAtSlot(i);

                cur_count--;
            }
        }
    }

    public void NotifyEnterPlayer(S_WaitingRoomEnterNotify message)
    {
        SpawnOtherCharacter(message);

        cur_count++;
    }

    public void SetMaKeRoom(RoomInfo roomInfo)
    {
        InitializeRoomInfo(roomInfo);

        IsOnRoomLeader();

        SpawnMyCharacter();
    }

    public void SetEnterRoom(RoomInfo roomInfo)
    {
        InitializeRoomInfo(roomInfo);

        SpawnMyCharacter();
    }

    private void InitializeRoomInfo(RoomInfo roomInfo)
    {
        titleName = roomInfo.Name;

        roomNumber = roomInfo.Num;

        roomState = roomInfo.RoomState;

        cur_count = roomInfo.CurCount;

        max_count = roomInfo.MaxCount;

        titleText.text = $"[{roomNumber}] {titleName}";

        playerInfos = new List<PlayerInfo>(max_count);

        for (int i = 0; i < max_count; i++)
        {
            playerInfos.Add(new PlayerInfo());  // 각 인덱스에 PlayerInfo 객체 추가

            readyImages[i].SetActive(false);
        }

        spawnSlotsOccupied = new bool[max_count];
    }

    public void Receive_WaitRoomOut(S_WaitingRoomOut message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            lobbyController.SetEnableControl(isLobby: true, isWaitRoom: false);
        }
        else
        {

        }

        isExit = false;
    }

    // MyCharacter 호출
    private void SpawnMyCharacter()
    {
        var myId = MercuryHelper.mercuryId;

        for (int i = 0; i < max_count; i++)
        {
            if (!spawnSlotsOccupied[i])
            {
                spawnIndex = i;

                PlayerController controller = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[spawnIndex]);

                controller.enabled = false;
                controller.rb.useGravity = false;
                controller.GetComponent<NetworkPlayerTransform>().enabled = false;

                playerInfos[spawnIndex].playerController = controller;
                playerInfos[spawnIndex].objectInfo.Id = myId;
                spawnSlotsOccupied[spawnIndex] = true;
                break;
            }
        }

        CheckAllPlayersReady();
    }

    // OtherCharacter 호출 (메시지에서 가져온 Id 사용)
    public void SpawnOtherCharacter(S_WaitingRoomEnterNotify message)
    {
        var otherId = message.Player.Id;

        for (int i = 0; i < max_count; i++)
        {
            if (!spawnSlotsOccupied[i])
            {
                PlayerController controller = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);

                controller.enabled = false;
                controller.rb.useGravity = false;
                controller.GetComponent<NetworkPlayerTransform>().enabled = false;

                playerInfos[i].playerController = controller;
                playerInfos[i].objectInfo.Id = otherId;
                spawnSlotsOccupied[i] = true;
                break;
            }
        }

        CheckAllPlayersReady();
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

    public void OnClickReady()
    {
        isReady = !isReady;

        playerInfos[spawnIndex].isReady = isReady;

        // 본인 자리 readyImage 반영
        readyImages[spawnIndex]?.SetActive(isReady);

        // 버튼 텍스트 반영
        readyButtonText.text = isReady ? "취 소" : "준 비";

        // 패킷 전송
        C_ChangeWaitingState packet = new C_ChangeWaitingState
        {
            State = isReady ? EWaitingState.WaitingStateRaedy : EWaitingState.WaitingStateRaedyCancle
        };

        NetworkManager.Instance.Send(packet);
    }

    public void OnClickStart()
    {
        // 상태가 "게임 시작"이면 -> 시작 로직 실행
        if (isRoomLeader && isAllReady)
        {
            Debug.Log("게임 시작");

            readyButton.interactable = false;

            gameStartButton.interactable = false;

            lobbyController.OnStartGame();

            return;
        }
    }

    private void OnClickExit()
    {
        if (isExit) return;

        isExit = true;

        C_WaitingRoomOut roomOut = new C_WaitingRoomOut();

        NetworkManager.Instance.Send(roomOut);
    }

    /// <summary>
    /// 내가 쏘면 나한테 옴.
    /// </summary>
    /// <param name="message"></param>
    private void OnReceiveChangeWaitingState(S_ChangeWaitingState message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {

        }
        else
        {
            // 실패 로직.. 
        }

        CheckAllPlayersReady();
    }

    /// <summary>
    /// 내가 쏘면 다른 클라한테 감.
    /// </summary>
    /// <param name="message"></param>
    private void OnReceiveChangeWaitingStateNotify(S_ChangeWaitingStateNotify message)
    {
        bool ready = (message.State == EWaitingState.WaitingStateRaedy);
        
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].objectInfo.Id == message.Player.Id)
            {
                playerInfos[i].isReady = ready;
                readyImages[i]?.SetActive(ready);
                break;
            }
        }

        CheckAllPlayersReady();
    }

    /// <summary>
    /// 룸 리더일시 체크해야 할 것
    /// </summary>
    private void CheckAllPlayersReady()
    {
        if (!isRoomLeader)
            return;
        
        int readyCount = 0;

        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].playerController != null && playerInfos[i].isReady)
            {
                readyCount++;
            }
        }

        if (readyCount == cur_count)
        {
            gameStartButton.interactable = true;
            isAllReady = true;
        }
        else
        {
            readyButtonText.text = isReady ? "취 소" : "준 비";
            gameStartButton.interactable = false;
            isAllReady = false;
        }
    }
}