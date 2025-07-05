using System.Threading.Tasks;
using Protocol;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;


namespace Assets.Scripts.Network.Handler
{
    public partial class PacketHandler
	{
		private void _Process_S_EnterLobby_Handler( ushort protocolId, byte[] data )
		{
			S_EnterLobby message = S_EnterLobby.Parser.ParseFrom( data );

            PacketEventManager.Invoke(message);
        }
    }
}