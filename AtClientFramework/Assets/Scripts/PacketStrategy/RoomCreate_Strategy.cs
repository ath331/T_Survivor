using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class RoomCreate_Strategy : IStrategy
{
    public static event Action<S_MakeRoom> OnRoomCreateReceived;

    public RoomCreate_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_MakeRoom>(OnCreateRoomPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_MakeRoom>(OnCreateRoomPacketReceived);
    }

    private void OnCreateRoomPacketReceived(S_MakeRoom message)
    {
        OnRoomCreateReceived?.Invoke(message);
    }
}
