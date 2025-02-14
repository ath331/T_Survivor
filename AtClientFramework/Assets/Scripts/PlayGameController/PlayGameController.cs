using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구체적인 게임 컨트롤러 구현: 실제 Play 씬에서 동작할 컨트롤러
public class PlayGameController : AbstractPlayGameController
{
    // 씬이 로드될 때 호출되는 초기화 함수
    private void Awake()
    {
        // 게임과 멀티플레이어 관련 초기화 실행
        InitializeGame();

        SetupMultiplayer();
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
}
