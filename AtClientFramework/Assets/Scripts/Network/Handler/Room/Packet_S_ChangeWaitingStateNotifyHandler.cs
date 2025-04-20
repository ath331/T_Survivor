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
		private void _Process_S_ChangeWaitingStateNotify_Handler( ushort protocolId, byte[] data )
		{
			S_ChangeWaitingStateNotify message = S_ChangeWaitingStateNotify.Parser.ParseFrom( data );

			PacketEventManager.Invoke( message );
		}
	}
}