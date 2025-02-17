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
    /// �� ���� �ʱ�ȭ�� �����մϴ�.
    /// �� ���������� 1�� �������� 10�ܰ� �����ϸ�, �� 1�� ���� �ʱ�ȭ�� ����ȴٰ� �����մϴ�.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("LobbyScene �ʱ�ȭ ����");

        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            // ���� ��Ȳ ���� (0~1 ����)
            progress.Report(i / (float)steps);
            await UniTask.Delay(100);  // ���� �ʱ�ȭ �۾� ��� 100ms ���
        }

        Debug.Log("LobbyScene �ʱ�ȭ �Ϸ�");

        // TODO : �ʱ�ȭ �Ϸ� �� ���� ���� �� �߰� �۾� ����
        
    }

    public void LoadPlayScene()
    {
        SwitchSceneManager.Instance.ChangeTo("Game").Forget();
    }
}
