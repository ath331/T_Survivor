using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LobbyController : MonoBehaviour, ISceneInitializer
{
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

    public void LoadPlayScene()
    {
        SwitchSceneManager.Instance.ChangeTo("Game").Forget();
    }
}
