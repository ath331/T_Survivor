using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class RoomDestroy_Strategy : IStrategy
{
    public static Action<S_DestroyRoom> OnRoomDestroyReceived;

    public RoomDestroy_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_DestroyRoom>(OnDestroyRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_DestroyRoom>(OnDestroyRoomPacketReceived);
    }

    private void OnDestroyRoomPacketReceived(S_DestroyRoom message)
    {
        OnRoomDestroyReceived?.Invoke(message);
    }
}
