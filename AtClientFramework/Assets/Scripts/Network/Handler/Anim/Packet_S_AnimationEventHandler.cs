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
		private void _Process_S_AnimationEvent_Handler( ushort protocolId, byte[] data )
		{
			S_AnimationEvent message = S_AnimationEvent.Parser.ParseFrom( data );

			var playerId = message.PlayerId;
			string animationType = message.AnimationType;
			EAnimationParamType paramType = message.ParamType;

            if (PlayerListManager.Instance.TryGetPlayer(playerId, out PlayerController player))
            {
                player.PlayNetworkAnimation(animationType, paramType, message.BoolValue);
            }
        }
	}
}