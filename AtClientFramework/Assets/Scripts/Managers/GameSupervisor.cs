using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    public async UniTask Test_ToLobby(int id)
    {
        // MercuryHelper를 통해 mercuryId 를 부여 받는다.
        await MercuryHelper.LoginProcess(id);

        await SwitchSceneManager.Instance.ChangeTo("Lobby");
    }
}
