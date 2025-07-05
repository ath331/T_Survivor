using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Cysharp.Threading.Tasks;
using Protocol;
using UnityEngine;

public class TestGameController : MonoBehaviour
{
    readonly string local_Ip = "127.0.0.1";
    readonly string local_Port = "7777";

    private StrategyManager strategyManager;

    private bool isStart = false;

    void Awake()
    {
        strategyManager = new StrategyManager();
        strategyManager.RegisterAllStrategies();

        NetworkManager.Instance.Initialize();

        PlayerListManager.Instance.Initialize();

        SoundManager.Initialize();

        ObjectPoolManager.Instance.Initialize();
    }

    void Start()
    {
        NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (isStart) return;

            Debug.Log("테스트 게임 시작! => 캐릭터 스폰");

            isStart = true;

            C_EnterGameFinish pkt = new C_EnterGameFinish();

            NetworkManager.Instance.Send(pkt);
        }
    }
}
