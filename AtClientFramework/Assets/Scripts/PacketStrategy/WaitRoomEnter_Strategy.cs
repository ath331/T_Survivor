using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class WaitRoomEnter_Strategy : IStrategy
{
    public static Action<S_WaitingRoomEnter> OnEnterRoom;

    public WaitRoomEnter_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_WaitingRoomEnter>(OnEnterRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_WaitingRoomEnter>(OnEnterRoomPacketReceived);
    }

    private void OnEnterRoomPacketReceived(S_WaitingRoomEnter message)
    {
        OnEnterRoom?.Invoke(message);
    }
}
