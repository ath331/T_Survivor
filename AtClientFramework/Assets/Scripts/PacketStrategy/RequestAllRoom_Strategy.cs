using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class RequestAllRoom_Strategy : IStrategy
{
    public static Action<S_RequestAllRoomInfo> OnRequestAllRoom;

    public RequestAllRoom_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_RequestAllRoomInfo>(OnRequestAllRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_RequestAllRoomInfo>(OnRequestAllRoomPacketReceived);
    }

    private void OnRequestAllRoomPacketReceived(S_RequestAllRoomInfo message)
    {
        OnRequestAllRoom?.Invoke(message);
    }
}
