using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class SwitchSceneManager : SingletonMonoBehaviour<SwitchSceneManager>
{
    [SerializeField] GameObject splashCanvas;
    [SerializeField] Image splashImage;

    [SerializeField] GameObject loadingCanvas;

    [SerializeField] ProgressBar progressBar;

    private Action OnComplete;

    private const float MinLoadingTime = 1f;

    protected override void Awake()
    {
        Initialize();
    }

    public override async void Initialize()
    {
        splashCanvas.SetActive(true);  // ���÷��� ȭ�� Ȱ��ȭ
        splashImage.color = new Color(0, 0, 0, 1);   // ���� �� ���� ��ο� (������)

        await StartSplashSequence();
    }

    /// <summary>
    /// ���÷��� ȭ�� ������ ���� (Fade In �� ���� �� Fade Out �� Title �� �ε�)
    /// </summary>
    private async UniTask StartSplashSequence()
    {
        // 1. Fade In (1.5�� ���� �����)
        await splashImage.DOFade(0f, 1.5f).AsyncWaitForCompletion();

        // 2. Fade Out (1.5�� ���� ��ο���)
        await splashImage.DOFade(1f, 1.5f).AsyncWaitForCompletion();

        await LoadTitleScene(); // Title �� �ε�
    }

    /// <summary>
    /// Title �� �ε� �� SplashCanvas ��Ȱ��ȭ
    /// </summary>
    private async UniTask LoadTitleScene()
    {
        await SceneManager.LoadSceneAsync("Title");
        splashCanvas.SetActive(false);
    }

    /// <summary>
    /// ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="param"></param>
    public async UniTask ChangeTo(string sceneName, Action complete = null)
	{
        progressBar.UpdateProgress(0f, 0f).Forget();

        bool minTimeOk = false;

        DOVirtual.DelayedCall(MinLoadingTime, () => minTimeOk = true);

        OnComplete = complete;

        loadingCanvas.SetActive(true);

        await Resources.UnloadUnusedAssets();

        await LoadSceneWithProgress(sceneName);

        await LoadSceneInitialize();

        await UniTask.Yield();

        await UniTask.WaitUntil(() => minTimeOk);

        OnComplete?.Invoke();

        // �ٲ� ���� �ʱ�ȭ�� ������ 1������ ��� �� �ε�ȭ���� ����.
        await UniTask.Yield();

        loadingCanvas.SetActive(false);
    }

    /// <summary>
    /// [1] �� �ε� �ܰ� (��ü ������� 0% ~ 50%) (�񵿱� �� �ε� + ����� �ݹ� ����)
    /// </summary>
    private async UniTask LoadSceneWithProgress(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.UpdateProgress(progress * 0.5f, 0.5f).Forget();

            if (operation.progress >= 0.9f)
            {
                // �� ��ȯ�� ���� �Ϸ�Ǹ� 1�� ��� �� �� Ȱ��ȭ
                await UniTask.Delay(TimeSpan.FromSeconds(1));

                operation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// [2] �� ���� �ʱ�ȭ �ܰ� (��ü ������� 50% ~ 100%)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private async UniTask LoadSceneInitialize()
    {
        ISceneInitializer initializer = null;

        // �ʱ�ȭ ������Ʈ�� ��ϵ� ������ ���
        await UniTask.WaitUntil(() =>
        {
            initializer = SceneInitializerRegistry.GetInitializer<ISceneInitializer>();
            return initializer != null;
        });

        if (initializer != null)
        {
            // progress ������: �ʱ�ȭ ����� 0~1�� 50~100%�� ����
            var sceneInitProgress = new Progress<float>(p =>
            {
                progressBar.UpdateProgress(0.5f + p * 0.5f, 0.5f).Forget();
            });

            await initializer.InitializeAsync(sceneInitProgress);
        }
    }
}
