using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class RequestAllRoom_Strategy : MonoBehaviour
{
    public RequestAllRoom_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_RequestAllRoomInfo>(ORequestAllRoomInfoPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_RequestAllRoomInfo>(ORequestAllRoomInfoPacketReceived);
    }

    private void ORequestAllRoomInfoPacketReceived(S_RequestAllRoomInfo message)
    {
        WaitingRoomHandler.OnRequestAllRoomInfo?.Invoke(message);
    }
}
