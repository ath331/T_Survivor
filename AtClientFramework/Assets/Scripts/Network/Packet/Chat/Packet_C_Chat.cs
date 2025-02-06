using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Network;
using Protocol;
using Google.Protobuf;
using TMPro;
using UnityEngine.Windows;


namespace Assets.Scripts.Network.Packet.Chat
{
	public class Packet_C_Chat : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputChat;

        // base.Start() block
        public void Start() {}

		public void Send_C_Chat()
		{
			C_Chat pkt = new C_Chat();

            string msg = inputChat.text;

            Network.Instance.Send( pkt );
		}
	}
}