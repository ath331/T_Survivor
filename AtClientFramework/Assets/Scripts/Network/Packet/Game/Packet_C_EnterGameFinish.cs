using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Network;
using Protocol;
using Google.Protobuf;


namespace Assets.Scripts.Network.Packet.Game
{
	public class Packet_C_EnterGameFinish
	{
		public void Send_C_EnterGameFinish()
		{
			C_EnterGameFinish pkt = new C_EnterGameFinish();

			NetworkManager.Instance.Send( pkt );
		}
	}
}