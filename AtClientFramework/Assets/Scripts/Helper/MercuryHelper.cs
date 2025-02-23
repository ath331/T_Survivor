using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MercuryHelper
{
    public enum LoginState
    {
        None,

        Try_Connect,
        Completed_Connect,

        Login_Invalid,
        Login_Fail,
    }

    private static LoginState currentState = LoginState.None;

    public static LoginState CurrentState
    {
        get => currentState;
        private set
        {
            currentState = value;
        }
    }

    public static int mercuryId = -1;

    public static bool IsReady => CurrentState == LoginState.Completed_Connect && mercuryId == -1;

    public static async UniTask LoginProcess(int id)
    {
        mercuryId = id;

        await UniTask.Delay(100);
    }
}
