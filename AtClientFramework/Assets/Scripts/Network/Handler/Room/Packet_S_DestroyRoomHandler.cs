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
		private void _Process_S_DestroyRoom_Handler( ushort protocolId, byte[] data )
		{
			S_DestroyRoom message = S_DestroyRoom.Parser.ParseFrom( data );

            PacketEventManager.Invoke(message);
        }
	}
}