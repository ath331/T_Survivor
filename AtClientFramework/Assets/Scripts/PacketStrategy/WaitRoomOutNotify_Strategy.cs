using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class WaitRoomOutNotify_Strategy : IStrategy
{
    public static Action<S_WaitingRoomOutNotify> OnRoomOutNotify;

    public WaitRoomOutNotify_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_WaitingRoomOutNotify>(OnNotifyRoomOutPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_WaitingRoomOutNotify>(OnNotifyRoomOutPacketReceived);
    }

    private void OnNotifyRoomOutPacketReceived(S_WaitingRoomOutNotify message)
    {
        OnRoomOutNotify?.Invoke(message);
    }
}
