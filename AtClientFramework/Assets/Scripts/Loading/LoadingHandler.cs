using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class LoadingHandler : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        SwitchSceneManager.Instance.OnSceneLoadStarted += LoadingStart;
        SwitchSceneManager.Instance.OnSceneLoadProgress += UpdateProgress;
    }

    private void OnDestroy()
    {
        if (SwitchSceneManager.Instance != null)
        {
            SwitchSceneManager.Instance.OnSceneLoadStarted -= LoadingStart;
            SwitchSceneManager.Instance.OnSceneLoadProgress -= UpdateProgress;
        }
    }

    private void LoadingStart()
    {
        progressBar.UpdateProgress(0f, 0).Forget();
    }

    private void UpdateProgress(float progress)
    {
        progressBar.UpdateProgress(progress, 0.5f).Forget();
    }
}
