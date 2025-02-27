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
        // S_EnterGame ���� �� �߻��ϴ� �̺�Ʈ
        public static event Action OnEnterGameSuccess;

        private void _Process_S_EnterGame_Handler( ushort protocolId, byte[] data )
		{
			S_EnterGame message = S_EnterGame.Parser.ParseFrom( data );


            // TODO : ���� / ���п� ���� �б�
            if (message.Result == EResultCode.ResultCodeSuccess)
            {
                Debug.Log("S_EnterGame: Success received.");

                // ���� �̺�Ʈ �߻�
                OnEnterGameSuccess?.Invoke();
            }
            else
            {
                Debug.LogWarning("S_EnterGame: Failure response received.");
                // ���� �� �߰� ó�� ���� (��: ���� �޽��� UI ǥ��)


            }
        }
	}
}