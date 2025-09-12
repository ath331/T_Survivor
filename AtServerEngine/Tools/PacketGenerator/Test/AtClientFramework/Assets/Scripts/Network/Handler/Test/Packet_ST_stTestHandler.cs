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
		private void _Process_ST_stTest_Handler( ushort protocolId, byte[] data )
		{
			ST_stTest message = ST_stTest.Parser.ParseFrom( data );

			PacketEventManager.Invoke( message );
		}
	}
}