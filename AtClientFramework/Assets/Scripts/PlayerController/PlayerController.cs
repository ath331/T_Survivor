using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;

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

    private ulong mercuryId = 0;

    // ���� ���� (�ʱ⿡�� Idle ����)
    private IPlayerState currentState;

    private Vector3 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // �ʱ� ���¸� IdleState�� ����
        ChangeState(new IdleState());

        mercuryId = MercuryHelper.mercuryId;

        Send_Move(transform.position);
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            // input update
            currentState.HandleInput(); 
        }

        // state update
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        // physics update
        currentState.FixedUpdateState();
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

    public void Send_Move(Vector3 pos)
    {
        if (!IsLocalPlayer) return;

        C_Move pkt = new C_Move
        {
            Info = new PosInfo
            {
                Id = mercuryId,

                X = pos.x,
                Y = pos.y,
                Z = pos.z,
            }
        };

        NetworkManager.Instance.Send(pkt);
    }

    /// <summary>
    /// ��Ʈ��ũ���� ���� ��ġ ������Ʈ (�ٸ� �÷��̾� ����)
    /// </summary>
    public void UpdateNetworkPosition(Vector3 newPosition)
    {
        if (!IsLocalPlayer)
        {
            targetPosition = newPosition;
        }
    }

    /// <summary>
    /// �ٸ� �÷��̾��� ��ġ�� ���� �����ͷ� �ε巴�� ���� �̵�
    /// </summary>
    public void SyncWithNetwork()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }
}
