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
	public class Packet_C_EnterGame
	{
		public void Send_C_EnterGame()
		{
			C_EnterGame pkt = new C_EnterGame();

			NetworkManager.Instance.Send( pkt );
		}
	}
}