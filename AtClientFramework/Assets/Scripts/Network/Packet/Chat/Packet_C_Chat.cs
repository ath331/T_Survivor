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
        public void Start() 
		{
			inputChat.onSubmit.AddListener((input) => Send_C_Chat());
		}

		public void Send_C_Chat()
		{
			string msg = inputChat.text.Trim();

			// 비어있는 메시지는 보내지 않음.
			if (string.IsNullOrEmpty(msg)) return;

			C_Chat pkt = new C_Chat();

			pkt.Msg = msg;

			inputChat.text = string.Empty;

			NetworkManager.Instance.Send( pkt );
		}
	}
}