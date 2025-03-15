using System;
using Protocol;
using UnityEngine;

public class Move_Strategy : IStrategy
{
    public Move_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_Move>(OnMovePacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_Move>(OnMovePacketReceived);
    }

    private void OnMovePacketReceived(S_Move message)
    {
        ulong playerId = message.ObjectInfo.Id;
        Vector3 newPosition = new Vector3(message.ObjectInfo.PosInfo.X, message.ObjectInfo.PosInfo.Y, message.ObjectInfo.PosInfo.Z);
        float newYaw = message.ObjectInfo.PosInfo.Yaw;

        if (PlayerListManager.Instance.TryGetPlayer(playerId, out PlayerController player))
        {
            if (!player.IsLocalPlayer)
            {
                player.UpdateNetworkPosition(newPosition, newYaw);
            }
        }
    }
}
