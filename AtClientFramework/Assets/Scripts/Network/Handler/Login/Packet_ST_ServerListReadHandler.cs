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
		private void _Process_ST_ServerListRead_Handler( ushort protocolId, byte[] data )
		{
			ST_ServerListRead message = ST_ServerListRead.Parser.ParseFrom( data );

			PacketEventManager.Invoke( message );
		}
	}
}