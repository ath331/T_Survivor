using UnityEngine;
using System.Collections.Generic;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // �ʱ� ���¸� IdleState�� ����
        ChangeState(new IdleState());
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
}
