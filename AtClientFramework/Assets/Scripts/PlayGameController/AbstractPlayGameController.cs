using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayGameController : MonoBehaviour, IPlayGameController
{
    // 하위 클래스에서 반드시 구현하게 함.
    public abstract void StartGame();
    public abstract void EndGame();

    // 공통 초기화 로직 (예: 리소스 로드, 게임 설정 등)
    protected virtual void InitializeGame()
    {
        Debug.Log("Game Initialized");

        // 게임 전반적인 초기화 작업을 여기에 작성.
    }

    // 멀티플레이어 관련 초기화(네트워크, 플레이어 매니저 등)
    protected virtual void SetupMultiplayer()
    {
        Debug.Log("Multiplayer Setup Completed");

        // 멀티플레이어 관련 초기화 작업을 여기에 작성.
    }
}
