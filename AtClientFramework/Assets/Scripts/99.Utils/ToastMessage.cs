using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] RectTransform scaleRoot;
    [SerializeField] TextMeshProUGUI messageText;

    private static Dictionary<string, ToastMessage> messageAnims = new Dictionary<string, ToastMessage>();

    public static ToastMessage Show(string message, Transform parent, string key = "", float dur = 1, Vector3 position = default)
    {
        // 같은 키로 실행된 애니메이션이 있다면, 기존 애니메이션 인스턴스를 제거한다.
        if (messageAnims.ContainsKey(key))
        {
            messageAnims[key]?.Clear();
        }

        var instance = ObjectPoolManager.Instance.Get<ToastMessage>(nameof(ToastMessage), parent);
        
        if (instance == null)
        {
            return null;
        }

        instance.Init(message, key);
        instance.ShowAsync(dur, position).Forget();
        return instance;
    }

    public static void Hide(string key)
    {
        // 같은 키로 실행된 애니메이션이 있다면, 기존 애니메이션 인스턴스를 제거한다.
        if (messageAnims.ContainsKey(key))
        {
            messageAnims[key]?.Clear();
        }
    }

    private string key;
    private CancellationTokenSource cancellationTokenSource;

    void Init(string message, string key = "")
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        messageText.text = message;
        scaleRoot.localScale = Vector3.zero;
        scaleRoot.localPosition = Vector3.zero;
        messageText.alpha = 1f;
        gameObject.SetActive(true);
        cancellationTokenSource = new CancellationTokenSource();

        if (string.IsNullOrEmpty(key))
        {
            key = GetInstanceID().ToString();
        }

        this.key = key;
    }

    async UniTask ShowAsync(float dur, Vector3 position)
    {
        if (position != default)
        {
            var mainCam = Camera.main;
            if (mainCam == null)
            {
                return;
            }

            var viewportPos = mainCam.WorldToViewportPoint(position);
            var canvasSizeDelta = canvasRectTransform.sizeDelta;
            var screenPosition = new Vector2(
                viewportPos.x * canvasSizeDelta.x - canvasSizeDelta.x * 0.5f,
                viewportPos.y * canvasSizeDelta.y - canvasSizeDelta.y * 0.5f);

            scaleRoot.anchoredPosition = screenPosition;
        }

        await DOTween.Sequence()
            .SetId(GetInstanceID())
            .Append(scaleRoot.DOLocalMoveY(50f, dur).SetRelative())
            .Join(scaleRoot.DOScale(1f, 0.2f))
            .Insert(dur * 0.8f, messageText.DOFade(0f, 0.2f))
            .OnStart(() => messageAnims[key] = this)
            .OnComplete(() => {
                if (!this || gameObject.activeSelf == false) return;

                ObjectPoolManager.Instance.Return(gameObject);

                if (messageAnims.ContainsKey(key)) messageAnims.Remove(key);
            })
            .AsUniTask(cancellationTokenSource.Token);
    }

    public Transform GetRootTransform() => scaleRoot;

    public void Clear()
    {
        cancellationTokenSource.Cancel();

        DOTween.Kill(GetInstanceID());

        ObjectPoolManager.Instance.Return(gameObject);
        
        if (messageAnims.ContainsKey(key)) messageAnims.Remove(key);
    }

    private void OnDisable()
    {
        DOTween.Kill(GetInstanceID());
    }
}
