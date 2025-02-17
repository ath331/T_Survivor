using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayGameController : MonoBehaviour, IPlayGameController
{
    // ���� Ŭ�������� �ݵ�� �����ϰ� ��.
    public abstract void StartGame();
    public abstract void EndGame();

    // ��Ƽ�÷��̾� ���� �ʱ�ȭ(��Ʈ��ũ, �÷��̾� �Ŵ��� ��)
    protected virtual void SetupMultiplayer()
    {
        Debug.Log("Multiplayer Setup Completed");

        // ��Ƽ�÷��̾� ���� �ʱ�ȭ �۾��� ���⿡ �ۼ�.
    }
}
