using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ʿ��� ��� �߰����� �޼���(��: PauseGame, ResumeGame ��)�� ������ �� ����.
public interface IPlayGameController
{
    void StartGame();
    
    void EndGame();
}

