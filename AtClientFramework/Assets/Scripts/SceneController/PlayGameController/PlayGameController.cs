using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network;
using Protocol;
using Google.Protobuf.WellKnownTypes;

// 구체적인 게임 컨트롤러 구현: 실제 Play 씬에서 동작할 컨트롤러
public class PlayGameController : AbstractPlayGameController, ISceneInitializer
{
    private void Awake()
    {
        SceneInitializerRegistry.Register(this);
    }


    private void OnDestroy()
    {
        SceneInitializerRegistry.Unregister(this);
    }

    // 게임 시작 로직 구현
    public override async UniTask StartGame()
    {
        // 예시: 플레이어 스폰, 게임 상태 전환, 멀티플레이어 세션 시작 등
        PacketEventManager.Subscribe<S_Move>(Receive_Move);
        PacketEventManager.Subscribe<S_AnimationEvent>(Receive_Animation);
        PacketEventManager.Subscribe<S_Spawn>(Receive_Spawn);

        // C_EnterGameFinish 를 전송한다.
        C_EnterGameFinish pkt = new C_EnterGameFinish();

        NetworkManager.Instance.Send(pkt);
    }

    // 게임 종료 로직 구현
    public override async UniTask EndGame()
    {
        // 예시: 게임 결과 처리, 데이터 저장, 네트워크 종료 등
        PacketEventManager.Unsubscribe<S_Move>(Receive_Move);
        PacketEventManager.Unsubscribe<S_AnimationEvent>(Receive_Animation);
        PacketEventManager.Unsubscribe<S_Spawn>(Receive_Spawn);
    }

    /// <summary>
    /// 씬 내부 초기화를 진행합니다.
    /// 이 예제에서는 1초 간격으로 10단계 진행하며, 총 1초 동안 초기화가 진행된다고 가정합니다.
    /// </summary>
    public async UniTask InitializeAsync(IProgress<float> progress)
    {
        Debug.Log("PlayScene 초기화 시작");

        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            // 진행 상황 갱신 (0~1 사이)
            progress.Report(i / (float)steps);
            await UniTask.Delay(100);  // 실제 초기화 작업 대신 100ms 대기
        }

        Debug.Log("PlayScene 초기화 완료");

        // 초기화 완료 후 게임 시작 등 추가 작업 수행
        StartGame();
    }

    private void Receive_Move(S_Move message)
    {
        ulong playerId = message.ObjectInfo.Id;
        var posInfo = message.ObjectInfo.PosInfo;

        // S_Move 위치 디버깅용
        {
            // 구체 생성
            GameObject sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            Vector3 pos = sphere.transform.position;
            pos.x = posInfo.X;
            pos.y = posInfo.Y;
            pos.z = posInfo.Z;

            sphere.transform.position = pos;

            // 색상(검정색) 적용
            Renderer renderer = sphere.GetComponent<Renderer>();
            renderer.material.color = Color.black;

            // 콜라이더 끄기
            SphereCollider collider = sphere.GetComponent<SphereCollider>();
            if ( collider != null )
                collider.enabled = false;
        }

        if (!PlayerListManager.Instance.TryGetPlayer( playerId, out var player)) return;
        if (player.IsLocalPlayer) return;

        player.networkPlayerTransform.SetTarget(posInfo.X, posInfo.Y, posInfo.Z, posInfo.Yaw);

    }

    // Receive_Animation 수정 제안
    private void Receive_Animation(S_AnimationEvent message)
    {
        if (!PlayerListManager.Instance.TryGetPlayer(message.PlayerId, out var player)) return;

        // 패킷의 ParamType에 따라 올바른 오버로드 메서드를 호출
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
        foreach (var objectInfo in message.ObjectList)
        {
            SpawnManager.Instance.ProcessSpawn(objectInfo);
        }
    }

    private void Receive_Despawn(S_DeSpawn message)
    {
        foreach (var objectId in message.Ids)
        {
            PlayerListManager.Instance.UnregisterPlayer(objectId);
        }
    }
}
