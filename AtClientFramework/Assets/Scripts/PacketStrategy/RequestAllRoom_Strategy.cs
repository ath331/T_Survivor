using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class RequestAllRoom_Strategy : IStrategy
{
    public RequestAllRoom_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_RequestAllRoomInfo>(OnRequestAllRoomInfoPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_RequestAllRoomInfo>(OnRequestAllRoomInfoPacketReceived);
    }

    private void OnRequestAllRoomInfoPacketReceived(S_RequestAllRoomInfo message)
    {
        WaitingRoomHandler.OnRequestAllRoomInfo?.Invoke(message);
    }
}
