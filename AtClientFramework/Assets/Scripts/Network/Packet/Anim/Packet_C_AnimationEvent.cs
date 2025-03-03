using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Network;
using Protocol;
using Google.Protobuf;


namespace Assets.Scripts.Network.Packet.Anim
{
	public class Packet_C_AnimationEvent : MonoBehaviour
	{
		// base.Start() block
		public void Start() {}

		public void Send_C_AnimationEvent()
		{
			C_AnimationEvent pkt = new C_AnimationEvent();

			Network.Instance.Send( pkt );
		}
	}
}