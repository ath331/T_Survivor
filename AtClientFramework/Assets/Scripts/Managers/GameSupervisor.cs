using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    public async UniTask Test_ToLobby(int id)
    {
        // MercuryHelper�� ���� mercuryId �� �ο� �޴´�.
        await MercuryHelper.LoginProcess(id);

        await SwitchSceneManager.Instance.ChangeTo("Lobby");
    }
}
