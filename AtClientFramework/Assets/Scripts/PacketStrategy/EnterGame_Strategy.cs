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
        // TODO : ���� / ���п� ���� �б�
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            Debug.Log("S_EnterGame: Success received.");

            // ���� �̺�Ʈ �߻�
            OnEnterGameSuccess?.Invoke();
        }
        else
        {
            Debug.LogWarning("S_EnterGame: Failure response received.");
            // ���� �� �߰� ó�� ���� (��: ���� �޽��� UI ǥ��)


        }
    }
}
