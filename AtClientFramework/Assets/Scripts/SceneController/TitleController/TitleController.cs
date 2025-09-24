using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using Assets.Scripts.Network;
using Unity.VisualScripting;
using Toggle = UnityEngine.UI.Toggle;
using Protocol;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class TitleController : MonoBehaviour, ISceneInitializer
{
    public struct ServerListData
    {
        public string Name;
        public string Ip;
        public int Port;

        public ServerListData(string name, string ip, int port)
        {
            Name = name;
            Ip = ip;
            Port = port;
        }
    }

    [SerializeField] private TMP_Dropdown dropdownServerList;

    private List<ServerListData> cachedServerList = new();

    public Toggle toggleLocal;

    [SerializeField] private Button connectButton;

    bool isCheckLocal = true;

    bool isReceivedServerList = false;

    readonly string titleServer_Ip = "211.210.246.35";
    readonly string titleServer_Port = "7778";
    readonly string local_Ip = "127.0.0.1";
    readonly string local_Port = "7777";

    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }

    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("TitleScene 초기화 시작");

        float currentProgress = 0f;

        // RoomManager 초기화 (가중치 0.2)
        RoomManager.Instance.Initialize();

        currentProgress += 0.2f;
        progress.Report(currentProgress);

        await UniTask.Delay(100);

        currentProgress += 0.8f;
        progress.Report(currentProgress);

        Debug.Log("TitleScene 초기화 완료");
    }

    void Start()
    {
        toggleLocal.onValueChanged.AddListener(OnToggleLocalValueChanged);
    }

    private void OnEnable()
    {
        PacketEventManager.Subscribe<ST_ServerListRead>(Receive_ServerListRead);
        PacketEventManager.Subscribe<S_EnterLobby>(Receive_EnterLobby);
    }

    private void OnDisable()
    {
        PacketEventManager.Unsubscribe<ST_ServerListRead>(Receive_ServerListRead);
        PacketEventManager.Unsubscribe<S_EnterLobby>(Receive_EnterLobby);
    }

    public async void OnConnectedToServer()
    {
        // 로컬 모드일 경우의 로직은 동일합니다.
        if (isCheckLocal)
        {
            await NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);
            return;
        }


        // UI 중복 클릭 방지
        connectButton.interactable = false;

        // 1. 아직 서버 목록을 받지 못했다면? -> 첫 번째 Connect 클릭
        if (!isReceivedServerList)
        {
            Debug.Log("First connect: Attempting to fetch server list from Title Server...");
            toggleLocal.interactable = false;
            try
            {
                // 타이틀 서버에 접속
                bool isConnected = await NetworkManager.Instance.ConnectToTcpServer(titleServer_Ip, titleServer_Port);

                if (isConnected)
                {
                    Debug.Log("Successfully connected to Title Server. Requesting server list...");

                    isReceivedServerList = true;

                    // 서버 목록 요청 패킷 전송
                    CT_ServerListRead c_packet = new CT_ServerListRead();
                    NetworkManager.Instance.Send(c_packet);
                }
                else
                {
                    Debug.LogError("Failed to connect to the Title Server.");

                    connectButton.interactable = true;
                }
            }
            finally
            {
                toggleLocal.interactable = true;
            }
        }
        // 2. 이미 서버 목록을 받았다면? -> 두 번째 Connect 클릭
        else
        {
            Debug.Log("Second connect: Attempting to connect to the selected game server...");
            int index = dropdownServerList.value;

            if (index >= 0 && index < cachedServerList.Count)
            {
                var selectedServer = cachedServerList[index];

                // 선택한 게임 서버로 새로 접속
               await NetworkManager.Instance.ReconnectToTcpServer(selectedServer.Ip, selectedServer.Port.ToString());
            }
        }
    }

    void OnToggleLocalValueChanged(bool isLocal)
    {
        isCheckLocal = isLocal;

        if (isLocal)
        {
            dropdownServerList.gameObject.SetActive(false);
            connectButton.interactable = true;
        }
        else
        {
            // 서버 모드일 때
            dropdownServerList.gameObject.SetActive(true);
            connectButton.interactable = true;
        }
    }

    public void Receive_ServerListRead(ST_ServerListRead message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            // 변환
            cachedServerList = message.ServerInfoList
                .Select(info => new ServerListData(info.Name, info.Ip, info.Port))
                .ToList();

            // Dropdown 초기화 및 채우기
            dropdownServerList.ClearOptions();
            dropdownServerList.AddOptions(cachedServerList.Select(data => data.Name).ToList());

            connectButton.interactable = cachedServerList.Any();
        }
    }

    public void Receive_EnterLobby(S_EnterLobby message)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        RoomManager.Instance.SetMyPlayerInfo(message.PlayerInfo);

        if (currentScene == "Test_Game")
        {
            Debug.Log("[테스트씬] 전용 로직 실행");
            MercuryHelper.LoginProcess(message.PlayerInfo.Id).Forget();
        }
        else
        {
            GameSupervisor.Instance.Test_ToLobby(message.PlayerInfo.Id).Forget();
        }
    }
}
