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
    /// 씬을 전환하는 함수
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="param"></param>
    public async UniTask ChangeTo(string sceneName)
	{
        // 로딩 씬을 로드
        await SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

        OnSceneLoadStarted?.Invoke();

        // 비동기 씬 로드
        await LoadSceneWithProgress(sceneName);
    }

    /// <summary>
    /// 비동기 씬 로드 + 진행률 콜백 제공
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
                // 씬 전환이 거의 완료되면 1초 대기 후 씬 활성화
                await UniTask.Delay(TimeSpan.FromSeconds(1));

                operation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }
    }
}
