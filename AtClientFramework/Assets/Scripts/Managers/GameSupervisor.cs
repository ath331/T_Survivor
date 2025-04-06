using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    private StrategyManager strategyManager;

    protected override void Awake()
    {
        base.Awake();

        strategyManager = new StrategyManager();
        strategyManager.RegisterAllStrategies();

        NetworkManager.Instance.Initialize();

        PlayerListManager.Instance.Initialize();

        SoundManager.Initialize();
    }

    public async UniTask Test_ToLobby(ulong id)
    {
        // MercuryHelper�� ���� mercuryId �� �ο� �޴´�.
        await MercuryHelper.LoginProcess(id);

        await SwitchSceneManager.Instance.ChangeTo("Lobby");
    }
}
