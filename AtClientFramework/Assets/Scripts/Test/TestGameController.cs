using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Cysharp.Threading.Tasks;
using Protocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameController : MonoBehaviour
{
    readonly string local_Ip = "127.0.0.1";
    readonly string local_Port = "7777";

    private bool isStart = false;

    void Awake()
    {
        NetworkManager.Instance.Initialize();

        PlayerListManager.Instance.Initialize();

        SoundManager.Initialize();

        ObjectPoolManager.Instance.Initialize();

        DataLoader.Instance.Initialize();
    }

    void Start()
    {
        NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);
    }

    private void OnEnable()
    {
        PacketEventManager.Subscribe<S_EnterLobby>(Receive_EnterLobby);
        PacketEventManager.Subscribe<S_Move>(Receive_Move);
        PacketEventManager.Subscribe<S_AnimationEvent>(Receive_Animation);
        PacketEventManager.Subscribe<S_Spawn>(Receive_Spawn);
    }

    private void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_EnterLobby>(Receive_EnterLobby);
        PacketEventManager.Unsubscribe<S_Move>(Receive_Move);
        PacketEventManager.Unsubscribe<S_AnimationEvent>(Receive_Animation);
        PacketEventManager.Unsubscribe<S_Spawn>(Receive_Spawn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (isStart) return;

            Debug.Log("테스트 게임 시작! => 캐릭터 스폰");

            isStart = true;

            C_EnterGameFinish pkt = new C_EnterGameFinish();

            NetworkManager.Instance.Send(pkt);
        }
    }

    private void Receive_Move(S_Move message)
    {
        if (!PlayerListManager.Instance.TryGetPlayer(message.ObjectInfo.Id, out var player)) return;
        if (player.IsLocalPlayer) return;

        ulong playerId = message.ObjectInfo.Id;

        var posInfo = message.ObjectInfo.PosInfo;

        player.networkPlayerTransform.SetTarget(posInfo.X, posInfo.Y, posInfo.Z, posInfo.Yaw);
    }

    private void Receive_Animation(S_AnimationEvent message)
    {
        if (!PlayerListManager.Instance.TryGetPlayer(message.PlayerId, out var player)) return;

        switch (message.ParamType)
        {
            case EAnimationParamType.AnimParamTypeBool:
                player.networkPlayerAnimation.SetAnimation(message.AnimationType, message.BoolValue);
                break;
            case EAnimationParamType.AnimParamTypeFloat:
                player.networkPlayerAnimation.SetAnimation(message.AnimationType, message.FloatValue);
                break;
            case EAnimationParamType.AnimParamTypeTrigger:
                player.networkPlayerAnimation.SetTrigger(message.AnimationType);
                break;
        }
    }

    private void Receive_Spawn(S_Spawn message)
    {
        var playerInfos = message.ObjectList;

        foreach (var playerInfo in playerInfos)
        {
            // 매니저에서 플레이어 생성 (중복 체크 포함)
            PlayerListManager.Instance.ProcessSpawnHandler(playerInfo);
        }
    }

    public void Receive_EnterLobby(S_EnterLobby message)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        MercuryHelper.LoginProcess(message.PlayerInfo.Id).Forget();
    }
}
