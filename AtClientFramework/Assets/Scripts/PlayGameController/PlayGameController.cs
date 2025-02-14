using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ü���� ���� ��Ʈ�ѷ� ����: ���� Play ������ ������ ��Ʈ�ѷ�
public class PlayGameController : AbstractPlayGameController
{
    // ���� �ε�� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    private void Awake()
    {
        // ���Ӱ� ��Ƽ�÷��̾� ���� �ʱ�ȭ ����
        InitializeGame();

        SetupMultiplayer();
    }

    // ���� ���� ���� ����
    public override async void StartGame()
    {
        Debug.Log("Game Started!");

        // ����: �÷��̾� ����, ���� ���� ��ȯ, ��Ƽ�÷��̾� ���� ���� ��
    }

    // ���� ���� ���� ����
    public override async void EndGame()
    {
        Debug.Log("Game Ended!");

        // ����: ���� ��� ó��, ������ ����, ��Ʈ��ũ ���� ��
    }
}
