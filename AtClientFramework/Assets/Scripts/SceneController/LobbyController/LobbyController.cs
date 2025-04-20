using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;
using Protocol;

public enum LobbyStatus
{
    WaitRoom,
    GameRoom,
}

public class LobbyController : MonoBehaviour, ISceneInitializer
{
    [SerializeField] private GameObject connectingPanel;
    [SerializeField] private WaitingRoomHandler waitingRoomHandler;
    [SerializeField] private LobbyHandler lobbyHandler;


    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }

    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    private void OnEnable()
    {
        EnterGame_Strategy.OnEnterGameSuccess += HandleEnterGameSuccess;

        RoomCreate_Strategy.OnRoomCreateReceived += CreateRoom;

        WaitRoomEnter_Strategy.OnEnterRoom += EnterRoom;

        WaitRoomNotify_Strategy.OnNotify += NotifyPlayer;
    }

    private void OnDisable()
    {
        EnterGame_Strategy.OnEnterGameSuccess -= HandleEnterGameSuccess;

        RoomCreate_Strategy.OnRoomCreateReceived -= CreateRoom;

        WaitRoomEnter_Strategy.OnEnterRoom -= EnterRoom;

        WaitRoomNotify_Strategy.OnNotify -= NotifyPlayer;
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("LobbyScene 초기화 시작");

        float currentProgress = 0f;

        // 1. ObjectPoolManager 초기화 (가중치 0.2)
        ObjectPoolManager.Instance.Initialize();
        currentProgress += 0.2f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 2. Lobby UI 에셋 로드 (가중치 0.3)
        SetEnableControl(isLobby: false, isWaitRoom: false);
        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 3. 네트워크 연결/초기화 (가중치 0.2)

        // 4. Lobby 데이터 초기화 (가중치 0.3)

        lobbyHandler.gameObject.SetActive(true);

        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        currentProgress += 0.2f;
        progress.Report(currentProgress);

        Debug.Log("LobbyScene 초기화 완료");
    }

    public void OnStartGameButtonClick()
    {
        // 접속중임을 알리는 패널 활성화
        connectingPanel.SetActive(true);

        C_EnterGame pkt = new C_EnterGame();

        NetworkManager.Instance.Send(pkt);
    }

    /// <summary>
    /// S_EnterGame 성공 이벤트 핸들러
    /// </summary>
    private void HandleEnterGameSuccess()
    {
        // 접속중 패널 비활성화
        connectingPanel.SetActive(false);

        // 게임 씬으로 전환
        SwitchSceneManager.Instance.ChangeTo("Game").Forget();
    }

    public void NotifyPlayer(S_WaitingRoomEnterNotify message)
    {
        waitingRoomHandler.NotifyEnterPlayer(message);
    }

    public void EnterRoom(S_WaitingRoomEnter message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("방들어가기 성공");

            SetEnableControl(isLobby: false, isWaitRoom: true);

            waitingRoomHandler.SetMaKeRoom(message.RoomInfo);
        }
        else
        {
            Debug.Log("방들어가기 실패");

        }
    }

    public void CreateRoom(S_MakeRoom message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("방만들기 성공");

            SetEnableControl(isLobby: false, isWaitRoom: true);

            waitingRoomHandler.SetMaKeRoom(message.MadeRoomInfo);

            waitingRoomHandler.IsOnRoomLeader();
        }
        else
        {
            Debug.Log("방만들기 실패");

        }
    }

    public void SetEnableControl(bool isLobby, bool isWaitRoom)
    {
        lobbyHandler.gameObject.SetActive(isLobby);

        waitingRoomHandler.gameObject.SetActive(isWaitRoom);
    }

    public void OnClickSetting()
    {
        
    }

    public void OnClickEquipment()
    {

    }

    public void OnClickExit()
    {

    }
}
