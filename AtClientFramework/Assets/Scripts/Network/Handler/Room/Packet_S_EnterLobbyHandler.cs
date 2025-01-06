using System.Threading.Tasks;
using Protocol;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;


namespace Assets.Scripts.Network.Handler
{
    public partial class PacketHandler
	{
		private async Task _Process_S_EnterLobby_Handler( ushort protocolId, byte[] data )
		{
			S_EnterLobby message = S_EnterLobby.Parser.ParseFrom( data );

			if ( message.Success )
			{
                await UniTask.SwitchToMainThread();
                SceneManager.LoadScene( "Lobby" );
			}
        }
    }
}