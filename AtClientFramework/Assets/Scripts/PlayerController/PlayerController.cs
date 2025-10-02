using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using Google.Protobuf.WellKnownTypes;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }

    [Header("정보 설정")]
    public WeaponController weapon; // 일단 대충.. 나중에 지울것
    public NetworkPlayerTransform networkPlayerTransform { get; private set; }
    public NetworkPlayerAnimation networkPlayerAnimation { get; private set; }
    public IJob CurrentJob { get; private set; }
    public IWeapon EquippedWeapon { get; private set; }
    public List<Skill> Skills { get; private set; } = new List<Skill>();

    public bool IsLocalPlayer { get; set; } // 내 캐릭터 여부 (NetworkManager에서 설정)

    // 현재 상태 (초기에는 Idle 상태)
    private IPlayerState currentState;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public readonly IPlayerState idleState = new IdleState();
    public readonly IPlayerState moveState = new MoveState();
    public readonly IPlayerState attackState = new AttackState();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        networkPlayerTransform = GetComponent<NetworkPlayerTransform>();
        networkPlayerAnimation = GetComponent<NetworkPlayerAnimation>();

        // 초기 상태를 IdleState로 설정
        ChangeState(idleState);

        targetPosition = Vector3.zero;
        targetRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            // input update
            currentState.HandleInput();

            // state update
            currentState.UpdateState();
        }
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            // physics update
            currentState.FixedUpdateState();
        }
    }

    /// <summary>
    /// 상태 전환 메서드
    /// </summary>
    public void ChangeState(IPlayerState newState)
    {
        // 현재 state Exit 
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        // 다음 state Enter
        if (currentState != null)
            currentState.Enter(this);
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Animation Event에 의해 호출될 실제 공격 판정 메서드
    /// </summary>
    public void OnAttackHit()
    {
        // weapon?.DealDamage();
    }

    /// <summary>
    /// AttackStateBehaviour에 의해 애니메이션이 끝날 때 호출될 메서드
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        // 공격이 끝나는 시점에 이동 입력이 있는지 확인
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 입력이 있으면 Move 상태로, 없으면 Idle 상태로 전환
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            ChangeState(moveState);
        }
        else
        {
            ChangeState(idleState);
        }
    }

    public void Send_Move()
    {
        if (!IsLocalPlayer) return;

        C_Move pkt = new C_Move
        {
            ObjectInfo = new ObjectInfo
            {
                Id = MercuryHelper.mercuryId,

                PosInfo = new PosInfo
                {
                    X = transform.position.x,
                    Y = transform.position.y,
                    Z = transform.position.z,

                    Yaw = transform.eulerAngles.y // (0 ~ 360)
                }
            }
        };

        NetworkManager.Instance.Send(pkt);
    }

    public void Send_Anim<T>(EAnimationParamType paramType, string animationType, T value = default)
    {
        C_AnimationEvent pkt = new C_AnimationEvent
        {
            ParamType = paramType,
            AnimationType = animationType
        };

        switch (paramType)
        {
            case EAnimationParamType.AnimParamTypeBool:
                if (value is bool boolVal)
                {
                    pkt.BoolValue = boolVal;
                    animator.SetBool(animationType, boolVal);
                }
                break;
            case EAnimationParamType.AnimParamTypeFloat:
                if (value is float floatVal)
                {
                    //pkt.FloatValue = floatVal;
                    animator.SetFloat(animationType, floatVal);
                }
                break;

            case EAnimationParamType.AnimParamTypeTrigger:
                animator.SetTrigger(animationType);
                break;
        }

        NetworkManager.Instance.Send(pkt);
    }
}