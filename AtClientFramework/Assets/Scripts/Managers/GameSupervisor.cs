using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameSupervisor : SingletonMonoBehaviour<GameSupervisor>
{
    public static string uniqueId = "";

    protected override void Awake()
    {
        SwitchSceneManager.Instance.Initialize();
    }

    public async UniTask Init()
    {
        // MercuryHelper�� ���� mercuryId �� �ο� �޴´�.

        
    }
}
