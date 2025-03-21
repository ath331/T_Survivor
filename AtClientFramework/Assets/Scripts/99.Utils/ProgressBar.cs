using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TMP_Text progressText;

    public async UniTask UpdateProgress(float targetValue, float duration)
    {
        // 프로그래스 바를 부드럽게 목표값까지 진행 (DOTween)
        await DOTween.To(() => progress.fillAmount, x => progress.fillAmount = x, targetValue, duration)
            .OnUpdate(() =>
            {
                // 값이 변경될 때마다 텍스트 업데이트
                if (progressText) progressText.text = $"{(int)(progress.fillAmount * 100)} / 100";
            }).AsyncWaitForCompletion();  // UniTask로 기다림
    }
}
