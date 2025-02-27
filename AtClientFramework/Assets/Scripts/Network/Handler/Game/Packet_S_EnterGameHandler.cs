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
        // S_EnterGame 성공 시 발생하는 이벤트
        public static event Action OnEnterGameSuccess;

        private void _Process_S_EnterGame_Handler( ushort protocolId, byte[] data )
		{
			S_EnterGame message = S_EnterGame.Parser.ParseFrom( data );


            // TODO : 성공 / 실패에 따라 분기
            if (message.Result == EResultCode.ResultCodeSuccess)
            {
                Debug.Log("S_EnterGame: Success received.");

                // 성공 이벤트 발생
                OnEnterGameSuccess?.Invoke();
            }
            else
            {
                Debug.LogWarning("S_EnterGame: Failure response received.");
                // 실패 시 추가 처리 가능 (예: 에러 메시지 UI 표시)


            }
        }
	}
}