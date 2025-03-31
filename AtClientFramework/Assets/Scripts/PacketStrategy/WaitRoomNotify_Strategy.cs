using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class WaitRoomNotify_Strategy : IStrategy
{
    public static Action<S_WaitingRoomEnterNotify> OnNotify;

    public WaitRoomNotify_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_WaitingRoomEnterNotify>(OnNotifyRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_WaitingRoomEnterNotify>(OnNotifyRoomPacketReceived);
    }

    private void OnNotifyRoomPacketReceived(S_WaitingRoomEnterNotify message)
    {
        OnNotify?.Invoke(message);
    }
}
