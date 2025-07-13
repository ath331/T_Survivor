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

    // private bool isReady = false;

    void OnEnable()
    {
        WaitRoomOutNotify_Strategy.OnRoomOutNotify += NotifyRoomOutPlayer;

        WaitRoomOut_Strategy.OnRoomOut += Receive_WaitRoomOut;

        exitButton.onClick.AddListener(OnClickExit);

        readyButton.onClick.AddListener(OnClickReady);

        PacketEventManager.Subscribe<S_ChangeWaitingState>(OnReceiveChangeWaitingState);
        PacketEventManager.Subscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeWaitingStateNotify);

        isExit = false;
    }

    void OnDisable()
    {
        WaitRoomOutNotify_Strategy.OnRoomOutNotify -= NotifyRoomOutPlayer;

        WaitRoomOut_Strategy.OnRoomOut -= Receive_WaitRoomOut;

        exitButton.onClick.RemoveAllListeners();

        Destroy_AllCharacter();

        isRoomLeader = false;

        readyButton.onClick.RemoveAllListeners();

        PacketEventManager.Unsubscribe<S_ChangeWaitingState>(OnReceiveChangeWaitingState);
        PacketEventManager.Unsubscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeWaitingStateNotify);
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
        SetRoomInfo(roomInfo);

        IsOnRoomLeader();

        titleText.text = $"[{roomNumber}] {titleName}";

        playerInfos = new List<PlayerInfo>(max_count);

        for (int i = 0; i < max_count; i++)
        {
            playerInfos.Add(new PlayerInfo());  // 각 인덱스에 PlayerInfo 객체 추가

            readyImages[i].SetActive(false);
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
        // 상태가 "게임 시작"이면 -> 시작 로직 실행
        if (isRoomLeader && readyButton.GetComponentInChildren<TMP_Text>().text == "게임 시작")
        {
            Debug.Log("방장이 게임 시작을 눌렀습니다. 시작 패킷 전송 로직 작성 필요");
            // 또는 다음 단계에 맞게 구현
            readyButton.interactable = false;
            return;
        }

        // 본인 PlayerInfo 가져오기
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].objectInfo.Id == MercuryHelper.mercuryId)
            {
                // 레디 상태 토글 후 서버에 전송
                playerInfos[i].isReady = !playerInfos[i].isReady;

                // 본인 자리 readyImage 즉시 반영
                readyImages[i]?.SetActive(playerInfos[i].isReady);

                // 버튼 텍스트도 즉시 반영
                readyButton.GetComponentInChildren<TMP_Text>().text = playerInfos[i].isReady ? " 취 소" : "준 비";

                // 패킷 전송
                C_ChangeWaitingState packet = new C_ChangeWaitingState
                {
                    State = playerInfos[i].isReady ? EWaitingState.WaitingStateRaedy : EWaitingState.WaitingStateRaedyCancle
                };

                NetworkManager.Instance.Send(packet);
                break;
            }
        }
    }


    private void OnClickExit()
    {
        if (isExit) return;

        isExit = true;

        C_WaitingRoomOut roomOut = new C_WaitingRoomOut();

        NetworkManager.Instance.Send(roomOut);
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

    private void OnReceiveChangeWaitingState(S_ChangeWaitingState message)
    {
        for (int i = 0; i < max_count; i++)
        {
            if (playerInfos[i].objectInfo.Id == MercuryHelper.mercuryId)
            {
                playerInfos[i].isReady = (message.State == EWaitingState.WaitingStateRaedy);
                readyButton.GetComponentInChildren<TMP_Text>().text = playerInfos[i].isReady ? "취 소" : "준 비";

                readyImages[i]?.SetActive(playerInfos[i].isReady);
                break;
            }
        }

        CheckAllPlayersReady();
    }

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
                Debug.Log(readyCount);
            }
        }

        if (readyCount == cur_count)
        {
            readyButton.GetComponentInChildren<TMP_Text>().text = "게임 시작";
        }
        else
        {
            for (int i = 0; i < max_count; i++)
            {
                if (playerInfos[i].objectInfo.Id == MercuryHelper.mercuryId)
                {
                    readyButton.GetComponentInChildren<TMP_Text>().text = playerInfos[i].isReady ? "취 소" : "준 비";
                    break;
                }
            }
        }
    }
    
}