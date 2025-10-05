using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;
using Protocol;

public enum LobbyStatus
{
    WaitRoom,
    Lobby,
}

public class LobbyController : MonoBehaviour, ISceneInitializer
{
    [SerializeField] private WaitingRoomHandler waitingRoomHandler;
    [SerializeField] private LobbyHandler lobbyHandler;

    private LobbyStatus currentStatus;
    private bool isInitialized = false;

    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }

    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    private async void OnEnable()
    {
        PacketEventManager.Subscribe<S_EnterGame>(HandleEnterGameSuccess);

        PacketEventManager.Subscribe<S_WaitingRoomOut>(OnReceiveWaitRoomOut);

        await UniTask.WaitUntil(() => RoomManager.Instance.isInitialized);

        RoomManager.Instance.OnChangeStatus += ChangeStatus;
    }

    private void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_EnterGame>(HandleEnterGameSuccess);

        PacketEventManager.Unsubscribe<S_WaitingRoomOut>(OnReceiveWaitRoomOut);

        RoomManager.Instance.OnChangeStatus -= ChangeStatus;
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("LobbyScene 초기화 시작");

        float currentProgress = 0f;

        // Manager 초기화 (가중치 0.2)
        if (!isInitialized)
        {
            ObjectPoolManager.Instance.Initialize();
            RoomManager.Instance.Initialize();
            DataLoader.Instance.Initialize();

            isInitialized = true;
        }

        currentProgress += 0.2f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 2. Lobby UI 에셋 로드 (가중치 0.3)
        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 3. 네트워크 연결/초기화 (가중치 0.2)

        // 4. Lobby 데이터 초기화 (가중치 0.3)

        //lobbyHandler.gameObject.SetActive(true);

        currentProgress += 0.3f;
        progress.Report(currentProgress);


        await UniTask.Delay(100);

        currentProgress += 0.2f;
        progress.Report(currentProgress);

        Debug.Log("LobbyScene 초기화 완료");
    }

    public void OnStartGame()
    {
        C_EnterGame pkt = new C_EnterGame();

        NetworkManager.Instance.Send(pkt);
    }

    /// <summary>
    /// S_EnterGame 성공 이벤트 핸들러
    /// </summary>
    private void HandleEnterGameSuccess(S_EnterGame message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            // 게임 씬으로 전환
            SwitchSceneManager.Instance.ChangeTo("Game").Forget();
        }
        else
        {
            // 실패 시 추가 처리 가능 (예: 에러 메시지 UI 표시)
            Debug.LogWarning("S_EnterGame: Failure response received.");
        }
    }

    public void ChangeStatus(LobbyStatus newStatus)
    {
        currentStatus = newStatus;

        switch (currentStatus)
        {
            case LobbyStatus.WaitRoom:
                lobbyHandler.gameObject.SetActive(false);
                waitingRoomHandler.gameObject.SetActive(true);
                break;
            case LobbyStatus.Lobby:
                lobbyHandler.gameObject.SetActive(true);
                waitingRoomHandler.gameObject.SetActive(false);
                break;
        }
    }

    private void OnReceiveWaitRoomOut(S_WaitingRoomOut message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            // 내가 방을 나갔으므로, 로비 화면으로 전환.
            ChangeStatus(LobbyStatus.Lobby);

            // 방을 나갔으니 RoomManager의 데이터도 초기화.
            RoomManager.Instance.roomModel.Clear();
        }
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
