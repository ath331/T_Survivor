using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// ���� ���� �˾��� ����Ǵ� ó���� ����ϴ� Ŭ����
/// �˾����� �ʼ������� BasePopupHandler�� ����� �޴´�.
/// </summary>
public class BasePopupHandler : MonoBehaviour
{
    private bool initialized;
    private int lockCount;

    private Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if (!initialized) Initialize();
            return canvas;
        }
    }

    private Transform animationRoot;
    protected virtual Transform AnimationRoot
    {
        get
        {
            if (!initialized) Initialize();
            return animationRoot;
        }
    }

    private void Initialize()
    {
        initialized = true;
        animationRoot = transform.Find("Canvas/PopupCommon");
        canvas = GetComponentInChildren<Canvas>(true);
    }

    protected virtual bool UseBlocker => true;             // ��濡 �ȴ����� �ϴ� blocker �� ä�����
    protected virtual bool HandleClickOutside => false;    // ������� �������� ���� �Ұ���
    protected virtual bool IsPointerDownHandler => true;   // ������� ��ġ �̺�Ʈ Ÿ�� (IPointerDownHandler / IPointerClickHandler)

    protected virtual bool HandleEscapeKey => true;
    protected virtual float CurtainAlpha => .8f;

    private const int BaseSortingOrder = 0;
    protected virtual int SortingGroupBonusOrder => 0;
    protected virtual string PopupSoundEnter => "popup_in";
    protected virtual string PopupSoundLeave => "popup_out";

    //�˾��� ���� ���� �� �ִ� �����ΰ�?
    protected virtual bool CanEscape()
    {
        // TODO : ������ �ڵ�
        // ���� ����� �ִ� �˾��̶� ���� �˾�����..? üũ ��

        return true;
    }

    /// <summary>
    /// �˾��� ������� �� (AnimationIn ��)
    /// </summary>
    /// <param name="param"></param>
    public virtual void OnBeforeEnter(object param)
    {
        //if (!string.IsNullOrEmpty(PopupSoundEnter)) SoundManager.Play(PopupSoundEnter);

        int popupCount = PopupManager.GetPopupStackCount();

        if (!ReferenceEquals(null, Canvas))
        {
            Canvas.sortingOrder = BaseSortingOrder + 1000 * popupCount;

            // if (UseBlocker) UIBlocker.Setup(GetInstanceID(), Canvas.sortingLayerName, Canvas.sortingOrder - 1, CurtainAlpha, IsPointerDownHandler);
        }
    }

    /// <summary>
    /// �˾��� ����� �� (AnimationIn ��)
    /// </summary>
    /// <param name="param"></param>
    public virtual void OnAfterEnter(object param)
    {

    }

    /// <summary>
    /// �˾��� ������ �� (AnimationOut ��)
    /// </summary>
    public virtual void OnBeforeLeave()
    {

    }

    /// <summary>
    /// �˾��� ���� �� (AnimtionOut ��)
    /// </summary>
    public virtual void OnAfterLeave()
    {
        
    }

    protected virtual void OnClickOutside()
    {
        
    }

    public virtual void OnClickEscape()
    {
        
    }

    public string GetName()
    {
        return name;
    }

    public virtual void OnClickClose()
    {
        PopupManager.ClosePopup();
    }

    public virtual void OnClickCloseReturnBool(bool b)
    {

    }

    /// <summary>
    /// TODO : �˾��� �����Ҷ��� �ִϸ��̼� (�˾��� �׳� �����°� �ƴ϶� ���� Ŀ���鼭 ���´ٴ��� �׷� ����!)
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask AnimationIn()
    {
        if (!AnimationRoot) return;

        var orgScale = Vector3.one;
        //var orgScale = AnimationRoot.localScale;
        var sequence = DOTween.Sequence()
            .Append(AnimationRoot.DOScale(orgScale * 1.02f, .12f).SetEase(Ease.OutQuart).From(orgScale))
            .Append(AnimationRoot.DOScale(orgScale * .99f, .08f).SetEase(Ease.OutQuart))
            .Append(AnimationRoot.DOScale(orgScale, .05f));

        await sequence.AwaitForCompletion();
    }

    public virtual async UniTask AnimationOut()
    {
        if (!AnimationRoot) return;

        var orgScale = Vector3.one;

        var sequence = DOTween.Sequence()
            .Append(AnimationRoot.DOScale(orgScale * 0.97f, 0.05f).SetEase(Ease.InQuad))
            .Append(AnimationRoot.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack));

        await sequence.AwaitForCompletion();
    }

    /// <summary>
    /// �˾��� ���� ������ �����ϴ� �޼���
    /// </summary>
    /// <param name="sortingOrder">���ο� ���� ����</param>
    public void AddSortingOrder(int sortingOrder)
    {

    }

    /// <summary>
    /// ���� �˾��� ���� ������ ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>���� ����</returns>
    public int GetSortingOrder()
    {
        return Canvas.sortingOrder;
    }
}