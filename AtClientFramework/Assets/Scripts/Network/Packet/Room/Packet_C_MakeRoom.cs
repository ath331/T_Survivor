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
	public class Packet_C_MakeRoom : MonoBehaviour
	{
		// base.Start() block
		public void Start() {}

		public void Send_C_MakeRoom()
		{
			C_MakeRoom pkt = new C_MakeRoom();

			Network.Instance.Send( pkt );
		}
	}
}