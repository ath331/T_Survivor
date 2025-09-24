using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Handler;
using Cysharp.Threading.Tasks;
using Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


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

    private Dictionary<ulong, PlayerController> spawnedCharacters = new Dictionary<ulong, PlayerController>();

    void OnEnable()
    {
        RoomManager.Instance.roomModel.OnModelUpdated += RefreshUI;

        // C_WaitingRoomOut 을 보내면 S_WaitingRoomOutNotify이 다른 클라한테 가고  S_WaitingRoomOut 는 본인한테 옴
        //PacketEventManager.Subscribe<S_WaitingRoomOutNotify>(NotifyRoomOutPlayer);
        //PacketEventManager.Subscribe<S_WaitingRoomOut>(Receive_WaitRoomOut);

        //PacketEventManager.Subscribe<S_ChangeWaitingState>(OnReceiveChangeWaitingState);
        //PacketEventManager.Subscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeWaitingStateNotify);

        exitButton.onClick.AddListener(OnClickExit);
        readyButton.onClick.AddListener(OnClickReady);
        gameStartButton.onClick.AddListener(OnClickStart);

        RefreshUI();
    }

    void OnDisable()
    {
        RoomManager.Instance.roomModel.OnModelUpdated -= RefreshUI;

        exitButton.onClick.RemoveAllListeners();
        readyButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.RemoveAllListeners();
    }

    public void RefreshUI()
    {
        var model = RoomManager.Instance.roomModel;
        
        if (model == null) return;

        // 1. 타이틀 업데이트
        titleText.text = $"[{model.RoomNumber}] {model.RoomName}";

        // 2. PlayerStateInfo 를 joinSequence에 따라 정렬.
        List<PlayerStateInfo> sortedPlayers = model.Players.Values.OrderBy(p => p.JoinSequence).ToList();

        // 3. 모델에 없는데 화면에 남아있는 캐릭터 제거
        List<ulong> toRemoveIds = spawnedCharacters.Keys.Except(sortedPlayers.Select(p => p.Info.Id)).ToList();
        
        foreach (var id in toRemoveIds)
        {
            if (spawnedCharacters.TryGetValue(id, out PlayerController controller))
            {
                controller.enabled = true;
                controller.rb.useGravity = true;
                ObjectPoolManager.Instance.Return(controller.gameObject);

                spawnedCharacters.Remove(id);
            }
        }

        // 3-2. 정렬된 목록을 기준으로 캐릭터 생성 및 배치
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            PlayerStateInfo currentPlayer = sortedPlayers[i];

            int slotIndex = i; // 정렬된 순서가 곧 슬롯 인덱스

            PlayerController characterController;
            
            if (!spawnedCharacters.TryGetValue(currentPlayer.Info.Id, out characterController))
            {
                // 화면에 캐릭터가 없으면 새로 스폰
                characterController = ObjectPoolManager.Instance.Get<PlayerController>("Knight");
                characterController.enabled = false; // 대기실에서는 PlayerController 비활성화
                characterController.networkPlayerTransform.enabled = false;
                characterController.rb.useGravity = false;
                spawnedCharacters[currentPlayer.Info.Id] = characterController;
            }

            // 항상 올바른 슬롯 위치에 있도록 위치 지정
            characterController.transform.SetParent(spawnTransform[slotIndex], false);
            characterController.transform.localPosition = Vector3.zero;
            characterController.transform.localRotation = Quaternion.identity;
        }

        // 4. 레디 상태 UI 동기화
        foreach (var img in readyImages) 
        { 
            img.SetActive(false); 
        }

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (sortedPlayers[i].IsReady)
            {
                // readyImages 배열의 범위를 벗어나지 않도록 방어.
                if (i < readyImages.Length)
                {
                    readyImages[i].SetActive(true);
                }
            }
        }

        // 5. 내 상태 및 버튼 UI 업데이트
        var myId = RoomManager.Instance.myPlayerInfo.Id;

        if (model.Players.TryGetValue(myId, out PlayerStateInfo myState))
        {
            readyButtonText.text = myState.IsReady ? "취 소" : "준 비";

            var isLeader = myState.IsLeader;

            gameStartButton.gameObject.SetActive(isLeader);
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
        var playerInfo = RoomManager.Instance.myPlayerInfo;

        var myState = RoomManager.Instance.roomModel.Players[playerInfo.Id];

        C_ChangeWaitingState packet = new C_ChangeWaitingState
        {
            State = myState.IsReady ? EWaitingState.WaitingStateRaedyCancle : EWaitingState.WaitingStateRaedy
        };

        NetworkManager.Instance.Send(packet);
    }

    public void OnClickStart()
    {
        var model = RoomManager.Instance.roomModel;

        bool allPlayersReady = model.Players.Values.All(p => p.IsReady);

        if (allPlayersReady)
        {
            lobbyController.OnStartGame();
        }
        else
        {
            ToastMessage.Show("모든 플레이어가 준비해야 시작할 수 있습니다.", transform);
        }
    }

    private void OnClickExit()
    {
        C_WaitingRoomOut packet = new C_WaitingRoomOut();

        NetworkManager.Instance.Send(packet);
    }
}