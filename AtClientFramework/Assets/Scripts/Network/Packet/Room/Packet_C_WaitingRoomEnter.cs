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
	public class Packet_C_WaitingRoomEnter : MonoBehaviour
	{
		// base.Start() block
		public void Start() {}

		public void Send_C_WaitingRoomEnter()
		{
			C_WaitingRoomEnter pkt = new C_WaitingRoomEnter();

			NetworkManager.Instance.Send( pkt );
		}
	}
}