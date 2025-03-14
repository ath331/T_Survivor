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
        ulong playerId = message.Info.Id;
        Vector3 newPosition = new Vector3(message.Info.X, message.Info.Y, message.Info.Z);
        float newYaw = message.Info.Yaw;

        if (PlayerListManager.Instance.TryGetPlayer(playerId, out PlayerController player))
        {
            if (!player.IsLocalPlayer)
            {
                player.UpdateNetworkPosition(newPosition, newYaw);
            }
        }
    }
}
