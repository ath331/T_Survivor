using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    protected override void Awake()
    {
        base.Awake();

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
}
