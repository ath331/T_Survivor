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

public class TitleController : MonoBehaviour
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
        if (isCheckLocal)
        {
            await NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);

            return;
        }

        if (isReceivedServerList)
        {
            // Dropdown에서 선택된 서버로 접속
            int index = dropdownServerList.value;

            if (index >= 0 && index < cachedServerList.Count)
            {
                var selectedServer = cachedServerList[index];

                await NetworkManager.Instance.ReconnectToTcpServer(selectedServer.Ip, selectedServer.Port.ToString());
            }
        }
    }

    async void OnToggleLocalValueChanged(bool isLocal)
    {
        isCheckLocal = isLocal;

        if (isLocal)
        {
            dropdownServerList.gameObject.SetActive(false);
            connectButton.interactable = true;
        }
        else
        {
            dropdownServerList.gameObject.SetActive(true);

            connectButton.interactable = false;

            if (!isReceivedServerList)
            {
                toggleLocal.interactable = false;

                try
                {
                    bool isConnected = await NetworkManager.Instance.ConnectToTcpServer(titleServer_Ip, titleServer_Port);

                    if (isConnected)
                    {
                        Debug.Log("Successfully connected to Title Server. Requesting server list...");

                        isReceivedServerList = true;

                        CT_ServerListRead c_packet = new CT_ServerListRead();

                        NetworkManager.Instance.Send(c_packet);
                    }
                    else
                    {
                        Debug.LogError("Failed to connect to the Title Server.");
                    }
                }
                finally
                {
                    // 토글은 다시 활성화
                    toggleLocal.interactable = true;
                }
            }
            else
            {
                // 이미 서버 목록이 있다면 버튼만 활성화
                connectButton.interactable = cachedServerList.Any();
            }
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

        if (currentScene == "Test_Game")
        {
            Debug.Log("[테스트씬] 전용 로직 실행");
            MercuryHelper.LoginProcess(message.PlayerId).Forget();
        }
        else
        {
            GameSupervisor.Instance.Test_ToLobby(message.PlayerId).Forget();
        }
    }
}
