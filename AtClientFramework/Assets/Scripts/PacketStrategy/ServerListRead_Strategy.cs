using Protocol;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

public class ServerListRead_Strategy : IStrategy
{
    public static Action<S_ServerListRead> OnServerListRead;

    public ServerListRead_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_ServerListRead>(OnServerListReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_ServerListRead>(OnServerListReceived);
    }

    private void OnServerListReceived(S_ServerListRead message)
    {
        OnServerListRead?.Invoke(message);
    }
}
