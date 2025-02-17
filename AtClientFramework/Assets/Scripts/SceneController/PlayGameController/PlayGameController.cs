using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

// ��ü���� ���� ��Ʈ�ѷ� ����: ���� Play ������ ������ ��Ʈ�ѷ�
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

    /// <summary>
    /// �� ���� �ʱ�ȭ�� �����մϴ�.
    /// �� ���������� 1�� �������� 10�ܰ� �����ϸ�, �� 1�� ���� �ʱ�ȭ�� ����ȴٰ� �����մϴ�.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("PlayScene �ʱ�ȭ ����");

        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            // ���� ��Ȳ ���� (0~1 ����)
            progress.Report(i / (float)steps);
            await UniTask.Delay(100);  // ���� �ʱ�ȭ �۾� ��� 100ms ���
        }

        Debug.Log("PlayScene �ʱ�ȭ �Ϸ�");

        // �ʱ�ȭ �Ϸ� �� ���� ���� �� �߰� �۾� ����
        StartGame();
    }
}
