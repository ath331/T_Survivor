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
    [SerializeField] private GameRoomHandler gameRoomHandler;
    [SerializeField] private WaitingRoomHandler waitingRoomHandler;

    public static Action<S_MakeRoom> OnRoomCreateAction;

    private bool isMakeMe = false;

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

        OnRoomCreateAction += ReceiveCreateRoom;
    }

    private void OnDisable()
    {
        EnterGame_Strategy.OnEnterGameSuccess -= HandleEnterGameSuccess;

        OnRoomCreateAction += ReceiveCreateRoom;
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
        waitingRoomHandler.gameObject.SetActive(false);
        gameRoomHandler.gameObject.SetActive(false);
        currentProgress += 0.3f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        // 3. ��Ʈ��ũ ����/�ʱ�ȭ (����ġ 0.2)

        // 4. Lobby ������ �ʱ�ȭ (����ġ 0.3)

        waitingRoomHandler.gameObject.SetActive(true);

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

    public void OnClickMakeRoom()
    {
        PopupManager.ShowPopup(nameof(MakeRoomPopup), null, (res) =>
        {
            if (res is C_MakeRoom)
            {
                isMakeMe = true;

                NetworkManager.Instance.Send(res as C_MakeRoom);
            }
        });
    }

    public void ReceiveCreateRoom(S_MakeRoom message)
    {
        if (isMakeMe)
        {
            CreateRoom(message);
        }
        else
        {
            CreateRoomHolder(message);
        }
    }

    public void CreateRoomHolder(S_MakeRoom message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            waitingRoomHandler.CreateRoomHolder(message);
        }
        else
        {
            // TODO : �� ��Ͽ� Holder ����� ����������

        }
    }

    public void CreateRoom(S_MakeRoom message)
    {
        isMakeMe = false;

        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("�游��� ����");

            waitingRoomHandler.gameObject.SetActive(false);

            gameRoomHandler.gameObject.SetActive(true);

            gameRoomHandler.SetMaKeRoom(message);
        }
        else
        {
            // TODO : �� ����� ����������

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
