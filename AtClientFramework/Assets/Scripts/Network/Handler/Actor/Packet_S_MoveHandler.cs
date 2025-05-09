using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;
using Protocol;
using UnityEngine;


namespace Assets.Scripts.Network.Handler
{
	public partial class PacketHandler
	{
        private void _Process_S_Move_Handler(ushort protocolId, byte[] data)
        {
            S_Move message = S_Move.Parser.ParseFrom(data);

            ulong playerId = message.Info.Id;
            Vector3 newPosition = new Vector3(message.Info.X, message.Info.Y, message.Info.Z);
            var newYaw = message.Info.Yaw;

            if (PlayerListManager.Instance.TryGetPlayer(playerId, out PlayerController player))
            {
                if (!player.IsLocalPlayer)
                {
                    player.UpdateNetworkPosition(newPosition, newYaw);
                }
            }
        }
    }
}