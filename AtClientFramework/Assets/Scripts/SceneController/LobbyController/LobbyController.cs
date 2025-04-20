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
    /// �� ���� �ʱ�ȭ�� �����մϴ�.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("LobbyScene �ʱ�ȭ ����");

        float currentProgress = 0f;

        // 1. ObjectPoolManager �ʱ�ȭ (����ġ 0.2)
        ObjectPoolManager.Instance.Initialize();
        currentProgress += 0.2f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 2. Lobby UI ���� �ε� (����ġ 0.3)
        SetEnableControl(isLobby: false, isWaitRoom: false);
        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 3. ��Ʈ��ũ ����/�ʱ�ȭ (����ġ 0.2)

        // 4. Lobby ������ �ʱ�ȭ (����ġ 0.3)

        lobbyHandler.gameObject.SetActive(true);

        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        currentProgress += 0.2f;
        progress.Report(currentProgress);

        Debug.Log("LobbyScene �ʱ�ȭ �Ϸ�");
    }

    public void OnStartGameButtonClick()
    {
        // ���������� �˸��� �г� Ȱ��ȭ
        connectingPanel.SetActive(true);

        C_EnterGame pkt = new C_EnterGame();

        NetworkManager.Instance.Send(pkt);
    }

    /// <summary>
    /// S_EnterGame ���� �̺�Ʈ �ڵ鷯
    /// </summary>
    private void HandleEnterGameSuccess()
    {
        // ������ �г� ��Ȱ��ȭ
        connectingPanel.SetActive(false);

        // ���� ������ ��ȯ
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
            Debug.Log("����� ����");

            SetEnableControl(isLobby: false, isWaitRoom: true);

            waitingRoomHandler.SetMaKeRoom(message.RoomInfo);
        }
        else
        {
            Debug.Log("����� ����");

        }
    }

    public void CreateRoom(S_MakeRoom message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("�游��� ����");

            SetEnableControl(isLobby: false, isWaitRoom: true);

            waitingRoomHandler.SetMaKeRoom(message.MadeRoomInfo);

            waitingRoomHandler.IsOnRoomLeader();
        }
        else
        {
            Debug.Log("�游��� ����");

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
