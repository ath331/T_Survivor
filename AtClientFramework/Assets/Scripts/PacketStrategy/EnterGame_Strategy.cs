using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnterGame_Strategy
{
    public static event Action OnEnterGameSuccess;

    public EnterGame_Strategy()
    {
        PacketEventManager.Subscribe<S_EnterGame>(OnEnterGamePacketReceived);
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
