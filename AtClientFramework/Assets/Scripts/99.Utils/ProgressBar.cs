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
        // ���α׷��� �ٸ� �ε巴�� ��ǥ������ ���� (DOTween)
        await DOTween.To(() => progress.fillAmount, x => progress.fillAmount = x, targetValue, duration)
            .OnUpdate(() =>
            {
                // ���� ����� ������ �ؽ�Ʈ ������Ʈ
                if (progressText) progressText.text = $"{(int)(progress.fillAmount * 100)} / 100";
            }).AsyncWaitForCompletion();  // UniTask�� ��ٸ�
    }
}
