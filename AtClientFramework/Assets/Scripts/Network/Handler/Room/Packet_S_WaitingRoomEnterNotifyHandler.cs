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
		private void _Process_S_WaitingRoomEnterNotify_Handler( ushort protocolId, byte[] data )
		{
			S_WaitingRoomEnterNotify message = S_WaitingRoomEnterNotify.Parser.ParseFrom( data );

			PacketEventManager.Invoke(message);
		}
	}
}