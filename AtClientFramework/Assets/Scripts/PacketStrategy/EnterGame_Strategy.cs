using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnterGame_Strategy : IStrategy
{
    public static event Action OnEnterGameSuccess;

    public EnterGame_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_EnterGame>(OnEnterGamePacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_EnterGame>(OnEnterGamePacketReceived);
    }

    private void OnEnterGamePacketReceived(S_EnterGame message)
    {
        // TODO : 성공 / 실패에 따라 분기
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("S_EnterGame: Success received.");

            // 성공 이벤트 발생
            OnEnterGameSuccess?.Invoke();
        }
        else
        {
            Debug.LogWarning("S_EnterGame: Failure response received.");
            // 실패 시 추가 처리 가능 (예: 에러 메시지 UI 표시)


        }
    }
}
