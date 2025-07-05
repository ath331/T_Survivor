using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnterLobby_Strategy : IStrategy
{
    public EnterLobby_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_EnterLobby>(OnEnterLobbyPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_EnterLobby>(OnEnterLobbyPacketReceived);
    }

    private void OnEnterLobbyPacketReceived(S_EnterLobby message)
    {
        if (message.Success)
        {
            GameSupervisor.Instance.Test_ToLobby(message.PlayerId).Forget();
        }
    }
}
