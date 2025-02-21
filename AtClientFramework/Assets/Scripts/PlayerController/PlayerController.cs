using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f;
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }

    [Header("Weapon ����")]
    public WeaponController weapon; // �ν����Ϳ��� �Ҵ�

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
        // input update
        currentState.HandleInput();

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
