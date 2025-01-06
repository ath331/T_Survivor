using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Network;
using Protocol;
using Google.Protobuf;


namespace Assets.Scripts.Network.Packet.Room
{
	public class Packet_C_EnterLobby : MonoBehaviour
	{
		// base.Start() block
		public void Start() {}

		public void Send_C_EnterLobby()
		{
			C_EnterLobby pkt = new C_EnterLobby();

			Network.Instance.Send( pkt );
		}
	}
}