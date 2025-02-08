using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PopupManager
{
    private class PopupItem
    {
        public readonly BasePopupHandler popupHandler;
        public readonly Action<object> closeCallback;
        public readonly object param;

        public PopupItem(BasePopupHandler popupHandler, Action<object> closeCallback, object param)
        {
            this.popupHandler = popupHandler;
            this.closeCallback = closeCallback;
            this.param = param;
        }
    }

    /// <summary>
    /// Popup���� �θ������Ʈ
    /// </summary>
    private static GameObject popupParent;
    private static GameObject PopupParent
    {
        get
        {
            if (popupParent) return popupParent;
            popupParent = new GameObject("Popup Parent");
            Object.DontDestroyOnLoad(popupParent);
            return popupParent;
        }
    }

    /// <summary>
    /// TODO : ���� ȭ���� �����ϴ� �˾��� �����Ѵ�.
    /// stack, queue, linkedlist ���� ����.
    /// </summary>
    private static readonly LinkedList<PopupItem> popupStack = new LinkedList<PopupItem>();


    public static int GetPopupStackCount() => popupStack.Count;

    // ���� �� �տ� �����ִ� �˾��� �����´�.
    //public static BasePopupHandler CurrentPopup
    //{
    //    get
    //    {
    //        if (popupStack.Count == 0) return null;

    //        return popupStack.Last().popupHandler;
    //    }
    //}

    /// <summary>
    /// �˾��� �ٷ� �����ְ������ ����.
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="callback"></param>
    public static void ShowPopup(string popupName, object param = null, System.Action<object> callback = null)
    {
        // TODO :
    }

    /// <summary>
    /// �˾��� await ShowAsync �� �����ְ� �� ���� ������ ��� ���⶧ ��
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="closeCallback"></param>
    public static async void ShowAsync(string popupName, object param = null, Action<object> closeCallback = null)
    {
        var done = false;

        try
        {
            // ShowProcessAsync�� ����Ǹ� done�� true�� ����
            ShowProcessAsync(popupName, param, closeParam =>
            {
                closeCallback?.Invoke(closeParam);  // closeCallback ����
                done = true;  // closeCallback�� ����� �� done�� true�� ����
            }).Forget();

            // done�� true�� �� ������ ���
            await UniTask.WaitUntil(() => done);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during popup show process: {ex.Message}");

            done = true;  // ���ܰ� �߻��ص� done�� true�� �����Ͽ� ��� ����
        }

        await UniTask.WaitUntil(() => done);
    }

    private static async UniTask ShowProcessAsync(string popupName, object param, System.Action<object> closeCallback = null)
    {
        BasePopupHandler popupInstance = null;

        // PopupName ������ �� BasePopupHandler ��ũ��Ʈ�� ���� �������� ������. 

        // ���д�.

        // ���� PopupParent ������ �־��ش�.

        // �˾� ���ÿ� �߰��Ѵ�.

        // ���� �ٲ���ٸ� ��ٸ���. (���� �ٲ��� Show)

        // OnEnable() -> Start()

        // �˾��� �����ϱ� ��

        // �˾��� ������ �ִϸ��̼�
        await popupInstance.AnimationOut();

        // �˾��� ������ ��

    }

    /// <summary>
    /// ���ÿ� �ִ� ��� �˾��� ����.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="destroy"></param>
    public static async void CloseAll(object param = null)
    {
       
    }

    /// <summary>
    /// ���ÿ� �ִ� �˾��� �̸��� ���� ù��°�� ã�Ƽ� ����.
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="destroy"></param>
    public static async void ClosePopupByName(string popupName, object param = null)
    {
        
    }

    public static async void ClosePopup(object param = null)
    {
        // �˾����� �ֻ��� ���� �����´�.

        // �����.

        // �ش� �˾��� ����.
    }

    private static async UniTask CloseProcessAsync(PopupItem popupItem, object param)
    {
        // �˾��� ������ ��

        // �˾��� ������ �ִϸ��̼�

        // �˾��� ���� ��

        // popup ������Ʈ�� ���� ��Ų��.

        // CloseCallback �� �ִٸ� �����Ѵ�.

    }
}