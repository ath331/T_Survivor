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
    /// Popup들의 부모오브젝트
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
    /// TODO : 현재 화면이 등장하는 팝업을 관리한다.
    /// stack, queue, linkedlist 뭐든 좋음.
    /// </summary>
    private static readonly LinkedList<PopupItem> popupStack = new LinkedList<PopupItem>();


    public static int GetPopupStackCount() => popupStack.Count;

    // 현재 맨 앞에 열려있는 팝업을 가져온다.
    //public static BasePopupHandler CurrentPopup
    //{
    //    get
    //    {
    //        if (popupStack.Count == 0) return null;

    //        return popupStack.Last().popupHandler;
    //    }
    //}

    /// <summary>
    /// 팝업을 바로 보여주고싶을때 쓴다.
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="callback"></param>
    public static void ShowPopup(string popupName, object param = null, System.Action<object> callback = null)
    {
        // TODO :
    }

    /// <summary>
    /// 팝업을 await ShowAsync 로 보여주고 그 이후 실행은 잠깐 멈출때 씀
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="closeCallback"></param>
    public static async void ShowAsync(string popupName, object param = null, Action<object> closeCallback = null)
    {
        var done = false;

        try
        {
            // ShowProcessAsync가 종료되면 done을 true로 설정
            ShowProcessAsync(popupName, param, closeParam =>
            {
                closeCallback?.Invoke(closeParam);  // closeCallback 실행
                done = true;  // closeCallback이 실행된 후 done을 true로 설정
            }).Forget();

            // done이 true가 될 때까지 대기
            await UniTask.WaitUntil(() => done);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during popup show process: {ex.Message}");

            done = true;  // 예외가 발생해도 done을 true로 설정하여 대기 종료
        }

        await UniTask.WaitUntil(() => done);
    }

    private static async UniTask ShowProcessAsync(string popupName, object param, System.Action<object> closeCallback = null)
    {
        BasePopupHandler popupInstance = null;

        // PopupName 프리팹 중 BasePopupHandler 스크립트를 가진 프리팹을 가져옴. 

        // 꺼둔다.

        // 만들어서 PopupParent 하위로 넣어준다.

        // 팝업 스택에 추가한다.

        // 씬이 바뀌었다면 기다린다. (씬이 바뀐후 Show)

        // OnEnable() -> Start()

        // 팝업이 등장하기 전

        // 팝업이 꺼지는 애니메이션
        await popupInstance.AnimationOut();

        // 팝업이 등장한 후

    }

    /// <summary>
    /// 스택에 있는 모든 팝업을 끈다.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="destroy"></param>
    public static async void CloseAll(object param = null)
    {
       
    }

    /// <summary>
    /// 스택에 있는 팝업을 이름을 통해 첫번째만 찾아서 끈다.
    /// </summary>
    /// <param name="popupName"></param>
    /// <param name="param"></param>
    /// <param name="destroy"></param>
    public static async void ClosePopupByName(string popupName, object param = null)
    {
        
    }

    public static async void ClosePopup(object param = null)
    {
        // 팝업스택 최상위 것을 가져온다.

        // 지운다.

        // 해당 팝업을 끈다.
    }

    private static async UniTask CloseProcessAsync(PopupItem popupItem, object param)
    {
        // 팝업이 꺼지기 전

        // 팝업이 꺼지는 애니메이션

        // 팝업이 꺼진 후

        // popup 오브젝트를 삭제 시킨다.

        // CloseCallback 이 있다면 실행한다.

    }
}