using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 필요한 경우 추가적인 메서드(예: PauseGame, ResumeGame 등)도 정의할 수 있음.
public interface IPlayGameController
{
    void StartGame();
    
    void EndGame();
}

