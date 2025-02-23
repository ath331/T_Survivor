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
        splashCanvas.SetActive(true);  // 스플래시 화면 활성화
        splashImage.color = new Color(0, 0, 0, 1);   // 시작 시 완전 어두움 (불투명)

        await StartSplashSequence();
    }

    /// <summary>
    /// 스플래시 화면 시퀀스 실행 (Fade In → 유지 → Fade Out → Title 씬 로드)
    /// </summary>
    private async UniTask StartSplashSequence()
    {
        // 1. Fade In (1.5초 동안 밝아짐)
        await splashImage.DOFade(0f, 1.5f).AsyncWaitForCompletion();

        // 2. Fade Out (1.5초 동안 어두워짐)
        await splashImage.DOFade(1f, 1.5f).AsyncWaitForCompletion();

        await LoadTitleScene(); // Title 씬 로드
    }

    /// <summary>
    /// Title 씬 로드 후 SplashCanvas 비활성화
    /// </summary>
    private async UniTask LoadTitleScene()
    {
        await SceneManager.LoadSceneAsync("Title");
        splashCanvas.SetActive(false);
    }

    /// <summary>
    /// 씬을 전환하는 함수
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

        // 바뀐 씬이 초기화될 때까지 1프레임 대기 후 로딩화면을 끈다.
        await UniTask.Yield();

        loadingCanvas.SetActive(false);
    }

    /// <summary>
    /// [1] 씬 로딩 단계 (전체 진행률의 0% ~ 50%) (비동기 씬 로드 + 진행률 콜백 제공)
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
                // 씬 전환이 거의 완료되면 1초 대기 후 씬 활성화
                await UniTask.Delay(TimeSpan.FromSeconds(1));

                operation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// [2] 씬 내부 초기화 단계 (전체 진행률의 50% ~ 100%)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private async UniTask LoadSceneInitialize()
    {
        ISceneInitializer initializer = null;

        // 초기화 컴포넌트가 등록될 때까지 대기
        await UniTask.WaitUntil(() =>
        {
            initializer = SceneInitializerRegistry.GetInitializer<ISceneInitializer>();
            return initializer != null;
        });

        if (initializer != null)
        {
            // progress 리포터: 초기화 진행률 0~1을 50~100%로 매핑
            var sceneInitProgress = new Progress<float>(p =>
            {
                progressBar.UpdateProgress(0.5f + p * 0.5f, 0.5f).Forget();
            });

            await initializer.InitializeAsync(sceneInitProgress);
        }
    }
}
