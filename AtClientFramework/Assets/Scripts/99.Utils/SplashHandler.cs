using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class SplashHandler : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TMP_Text progressText;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private async UniTask LoadingStart()
    {


        await UniTask.Yield();
    }
}
