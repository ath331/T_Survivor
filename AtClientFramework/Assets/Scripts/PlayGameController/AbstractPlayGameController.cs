using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayGameController : MonoBehaviour, IPlayGameController
{
    // ���� Ŭ�������� �ݵ�� �����ϰ� ��.
    public abstract void StartGame();
    public abstract void EndGame();

    // ���� �ʱ�ȭ ���� (��: ���ҽ� �ε�, ���� ���� ��)
    protected virtual void InitializeGame()
    {
        Debug.Log("Game Initialized");

        // ���� �������� �ʱ�ȭ �۾��� ���⿡ �ۼ�.
    }

    // ��Ƽ�÷��̾� ���� �ʱ�ȭ(��Ʈ��ũ, �÷��̾� �Ŵ��� ��)
    protected virtual void SetupMultiplayer()
    {
        Debug.Log("Multiplayer Setup Completed");

        // ��Ƽ�÷��̾� ���� �ʱ�ȭ �۾��� ���⿡ �ۼ�.
    }
}
