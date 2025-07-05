using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Network;
using UnityEngine;


namespace Assets.Scripts.Network.Handler
{
	public partial class PacketHandler
	{
		public void ProcessHandler( ushort protocolId, byte[] data )
		{
			EPacketId packetId = (EPacketId)( protocolId );
			switch ( packetId )
			{
			default:
				{
					Debug.LogWarning( $"Invalid Protocol ID : {protocolId}" );
				}
				break;
			}
		}
	}
}