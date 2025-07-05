using Protocol;
using System;
using UnityEngine;

public class Chat_Strategy : IStrategy
{
    public static Action<string> OnChatReceived;

    public Chat_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_Chat>(OnChatPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_Chat>(OnChatPacketReceived);
    }

    private void OnChatPacketReceived(S_Chat message)
    {
        Debug.Log(message.Msg);

        OnChatReceived?.Invoke(message.Msg);
    }
}
