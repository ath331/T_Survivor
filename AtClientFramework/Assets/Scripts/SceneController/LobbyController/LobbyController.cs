using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;

public class LobbyController : MonoBehaviour, ISceneInitializer
{
    [SerializeField] private GameObject connectingPanel;

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
        // S_EnterGame 성공 이벤트를 구독합니다.
        PacketHandler.OnEnterGameSuccess += HandleEnterGameSuccess;
    }

    private void OnDisable()
    {
        PacketHandler.OnEnterGameSuccess -= HandleEnterGameSuccess;
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// 이 예제에서는 1초 간격으로 10단계 진행하며, 총 1초 동안 초기화가 진행된다고 가정합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("LobbyScene 초기화 시작");

        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            // 진행 상황 갱신 (0~1 사이)
            progress.Report(i / (float)steps);
            await UniTask.Delay(100);  // 실제 초기화 작업 대신 100ms 대기
        }

        Debug.Log("LobbyScene 초기화 완료");

        // TODO : 초기화 완료 후 게임 시작 등 추가 작업 수행
        

    }

    public void OnStartGameButtonClick()
    {
        // 접속중임을 알리는 패널 활성화
        connectingPanel.SetActive(true);

        // Enter_Game 패킷 전송
        NetworkManager.Instance.Enter_Game();
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
}
