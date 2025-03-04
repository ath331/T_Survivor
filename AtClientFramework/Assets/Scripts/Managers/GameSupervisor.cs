using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    private List<object> strategies;

    protected override void Awake()
    {
        base.Awake();

        RegisterStrategies();

        NetworkManager.Instance.Initialize();

        ObjectPoolManager.Instance.Initialize();

        PlayerListManager.Instance.Initialize();
    }

    public async UniTask Test_ToLobby(ulong id)
    {
        // MercuryHelper를 통해 mercuryId 를 부여 받는다.
        await MercuryHelper.LoginProcess(id);

        await SwitchSceneManager.Instance.ChangeTo("Lobby");
    }

    private void RegisterStrategies()
    {
        strategies = new List<object>();

        strategies.Add(new Chat_Strategy());
        strategies.Add(new Login_Strategy());
        strategies.Add(new EnterGame_Strategy());
        strategies.Add(new EnterLobby_Strategy());
        strategies.Add(new Move_Strategy());
        strategies.Add(new Animation_Strategy());
        strategies.Add(new Spawn_Strategy());

        Debug.Log("모든 전략이 등록되었습니다.");
    }
}
