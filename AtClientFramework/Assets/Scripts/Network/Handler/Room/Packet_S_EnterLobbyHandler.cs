using System.Threading.Tasks;
using Protocol;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.Network.Handler
{
    public partial class PacketHandler
	{
		private void _Process_S_EnterLobby_Handler( ushort protocolId, byte[] data )
		{
			S_EnterLobby message = S_EnterLobby.Parser.ParseFrom( data );

			if ( message.Success )
			{
                SceneManager.LoadScene( "Lobby" );
			}
        }
    }
}