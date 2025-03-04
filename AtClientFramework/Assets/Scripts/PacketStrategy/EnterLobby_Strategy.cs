using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnterLobby_Strategy
{
    public EnterLobby_Strategy()
    {
        PacketEventManager.Subscribe<S_EnterLobby>(OnEnterLobbyPacketReceived);
    }

    private void OnEnterLobbyPacketReceived(S_EnterLobby message)
    {
        if (message.Success)
        {
            GameSupervisor.Instance.Test_ToLobby(message.PlayerId).Forget();
        }
    }
}
