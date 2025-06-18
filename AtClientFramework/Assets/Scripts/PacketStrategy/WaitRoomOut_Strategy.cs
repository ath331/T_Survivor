using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class WaitRoomOut_Strategy : IStrategy
{
    public static Action<S_WaitingRoomOut> OnRoomOut;

    public WaitRoomOut_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_WaitingRoomOut>(OnRoomOutPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_WaitingRoomOut>(OnRoomOutPacketReceived);
    }

    private void OnRoomOutPacketReceived(S_WaitingRoomOut message)
    {
        OnRoomOut?.Invoke(message);
    }
}
