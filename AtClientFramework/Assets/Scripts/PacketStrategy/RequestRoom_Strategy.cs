using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class RequestRoom_Strategy : IStrategy
{
    public static Action<S_RequestRoomInfo> OnRequestRoom;

    public RequestRoom_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_RequestRoomInfo>(OnRequestRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_RequestRoomInfo>(OnRequestRoomPacketReceived);
    }

    private void OnRequestRoomPacketReceived(S_RequestRoomInfo message)
    {
        OnRequestRoom?.Invoke(message);
    }
}
