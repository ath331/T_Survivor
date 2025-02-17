using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

// 구체적인 게임 컨트롤러 구현: 실제 Play 씬에서 동작할 컨트롤러
public class PlayGameController : AbstractPlayGameController, ISceneInitializer
{
    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }


    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    // 게임 시작 로직 구현
    public override async void StartGame()
    {
        Debug.Log("Game Started!");

        // 예시: 플레이어 스폰, 게임 상태 전환, 멀티플레이어 세션 시작 등
    }

    // 게임 종료 로직 구현
    public override async void EndGame()
    {
        Debug.Log("Game Ended!");

        // 예시: 게임 결과 처리, 데이터 저장, 네트워크 종료 등
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// 이 예제에서는 1초 간격으로 10단계 진행하며, 총 1초 동안 초기화가 진행된다고 가정합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("PlayScene 초기화 시작");

        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            // 진행 상황 갱신 (0~1 사이)
            progress.Report(i / (float)steps);
            await UniTask.Delay(100);  // 실제 초기화 작업 대신 100ms 대기
        }

        Debug.Log("PlayScene 초기화 완료");

        // 초기화 완료 후 게임 시작 등 추가 작업 수행
        StartGame();
    }
}
