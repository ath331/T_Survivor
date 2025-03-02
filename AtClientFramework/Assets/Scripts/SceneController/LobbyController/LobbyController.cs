using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;

public class LobbyController : MonoBehaviour, ISceneInitializer
{
    [SerializeField] private GameObject connectingPanel;

    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }

    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    private void OnEnable()
    {
        // S_EnterGame ���� �̺�Ʈ�� �����մϴ�.
        PacketHandler.OnEnterGameSuccess += HandleEnterGameSuccess;
    }

    private void OnDisable()
    {
        PacketHandler.OnEnterGameSuccess -= HandleEnterGameSuccess;
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

    public void OnStartGameButtonClick()
    {
        // ���������� �˸��� �г� Ȱ��ȭ
        connectingPanel.SetActive(true);

        // Enter_Game ��Ŷ ����
        NetworkManager.Instance.Enter_Game();
    }

    /// <summary>
    /// S_EnterGame ���� �̺�Ʈ �ڵ鷯
    /// </summary>
    private void HandleEnterGameSuccess()
    {
        // ������ �г� ��Ȱ��ȭ
        connectingPanel.SetActive(false);

        // ���� ������ ��ȯ
        SwitchSceneManager.Instance.ChangeTo("Game").Forget();
    }
}
