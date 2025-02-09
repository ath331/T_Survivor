using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneManager : SingletonMonoBehaviour<SwitchSceneManager>
{
    public Action OnSceneLoadStarted;

    public Action<float> OnSceneLoadProgress;

    /// <summary>
    /// ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="param"></param>
    public async UniTask ChangeTo(string sceneName)
	{
        // �ε� ���� �ε�
        await SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

        OnSceneLoadStarted?.Invoke();

        // �񵿱� �� �ε�
        await LoadSceneWithProgress(sceneName);
    }

    /// <summary>
    /// �񵿱� �� �ε� + ����� �ݹ� ����
    /// </summary>
    private async UniTask LoadSceneWithProgress(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            OnSceneLoadProgress?.Invoke(progress);

            if (operation.progress >= 0.9f)
            {
                // �� ��ȯ�� ���� �Ϸ�Ǹ� 1�� ��� �� �� Ȱ��ȭ
                await UniTask.Delay(TimeSpan.FromSeconds(1));

                operation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }
    }
}
