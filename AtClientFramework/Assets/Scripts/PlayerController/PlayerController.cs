using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using Google.Protobuf.WellKnownTypes;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f;
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }

    [Header("���� ����")]
    public WeaponController weapon; // �ϴ� ����.. ���߿� �����
    public IJob CurrentJob { get; private set; }
    public IWeapon EquippedWeapon { get; private set; }
    public List<Skill> Skills { get; private set; } = new List<Skill>();

    public bool IsLocalPlayer { get; set; } // �� ĳ���� ���� (NetworkManager���� ����)

    // ���� ���� (�ʱ⿡�� Idle ����)
    private IPlayerState currentState;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // �ʱ� ���¸� IdleState�� ����
        ChangeState(new IdleState());

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
        else
        {

        }
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            // physics update
            currentState.FixedUpdateState();
        }
        else
        {
            Sync_Move();
        }
    }

    /// <summary>
    /// ���� ��ȯ �޼���
    /// </summary>
    public void ChangeState(IPlayerState newState)
    {
        // ���� state Exit 
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        // ���� state Enter
        if (currentState != null)
            currentState.Enter(this);
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }

    public void Send_Move()
    {
        if (!IsLocalPlayer) return;

        C_Move pkt = new C_Move
        {
            Info = new PosInfo
            {
                Id = MercuryHelper.mercuryId,

                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z,

                Yaw = transform.eulerAngles.y // (0 ~ 360)
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

    /// <summary>
    /// ��Ʈ��ũ���� ���� ��ġ ������Ʈ (�ٸ� �÷��̾� ����)
    /// </summary>
    public void UpdateNetworkPosition(Vector3 newPosition, float newYaw)
    {
        if (IsLocalPlayer) return;

        targetPosition = newPosition;
        targetRotation = Quaternion.Euler(0, newYaw, 0);
    }

    /// <summary>
    /// �ٸ� �÷��̾��� ��ġ�� ���� �����ͷ� �ε巴�� ���� �̵�
    /// </summary>
    public void Sync_Move()
    {
        if (IsLocalPlayer) return;

        // Rigidbody�� ���� ��� MovePosition ���
        if (rb != null)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 10f));
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20f)); // ȸ�� ����
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20f); // ȸ�� ����
        }
    }

    public void PlayNetworkAnimation(string animationType, EAnimationParamType paramType, bool boolValue)
    {
        if (!IsLocalPlayer && animator != null)
        {
            switch (paramType)
            {
                case EAnimationParamType.AnimParamTypeBool:
                    animator.SetBool(animationType, boolValue);
                    break;
                    // TODO : float �߰��� ���ľ���
                case EAnimationParamType.AnimParamTypeFloat:
                    animator.SetFloat(animationType, 0f);
                    break;

                case EAnimationParamType.AnimParamTypeTrigger:
                    animator.SetTrigger(animationType);
                    break;
            }
        }
    }
}