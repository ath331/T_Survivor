using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using Assets.Scripts.Network;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;
using Protocol;
using System.Linq;
using UnityEngine.SceneManagement;

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

    bool isCheckLocal = true;

    bool isReceivedServerList = false;

    readonly string local_Ip = "127.0.0.1";
    readonly string local_Port = "7777";

    void Start()
    {
        toggleLocal.onValueChanged.AddListener(OnToggleLocalValueChanged);
    }

    private void OnEnable()
    {
        PacketEventManager.Subscribe<S_ServerListRead>(Receive_ServerListRead);
        PacketEventManager.Subscribe<S_EnterLobby>(Receive_EnterLobby);
    }

    private void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_ServerListRead>(Receive_ServerListRead);
        PacketEventManager.Unsubscribe<S_EnterLobby>(Receive_EnterLobby);
    }

    public void OnConnectedToServer()
    {
        if (isCheckLocal)
        {
            NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);

            return;
        }

        if (isReceivedServerList)
        {
            // Dropdown에서 선택된 서버로 접속
            int index = dropdownServerList.value;

            if (index >= 0 && index < cachedServerList.Count)
            {
                var selectedServer = cachedServerList[index];

                NetworkManager.Instance.ConnectToTcpServer(selectedServer.Ip, selectedServer.Port.ToString());
            }
        }
        else
        {
            isReceivedServerList = true;

            NetworkManager.Instance.ConnectToTcpServer("211.210.246.35", "7778");

            C_ServerListRead c_packet = new C_ServerListRead();

            NetworkManager.Instance.Send(c_packet);
        }
    }

    void OnToggleLocalValueChanged(bool val)
    {
        isCheckLocal = val;
    }

    public void Receive_ServerListRead(S_ServerListRead message)
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
