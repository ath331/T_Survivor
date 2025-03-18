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
		private void _Process_S_RequestRoomInfo_Handler( ushort protocolId, byte[] data )
		{
			S_RequestRoomInfo message = S_RequestRoomInfo.Parser.ParseFrom( data );
		}
	}
}