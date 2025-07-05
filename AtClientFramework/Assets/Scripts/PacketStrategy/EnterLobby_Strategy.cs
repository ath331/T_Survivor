using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

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
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Test_Game")
        {
            Debug.Log("[테스트씬] 전용 로직 실행");
            MercuryHelper.LoginProcess(message.PlayerId).Forget();
        }
        else
        {
            GameSupervisor.Instance.Test_ToLobby(message.PlayerId).Forget();
        }
    }
}
