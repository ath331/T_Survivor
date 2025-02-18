using UnityEngine;

public class MoveState : IPlayerState
{
    private PlayerController player;
    private Vector3 moveDirection;

    public void Enter(PlayerController player)
    {
        this.player = player;
        Debug.Log("Move ���� ����");
    }

    public void Exit()
    {
        // ���� ���� �� �Է� �ʱ�ȭ ��
        moveDirection = Vector3.zero;
    }

    public void HandleInput()
    {
        // WASD Ȥ�� ���̽�ƽ �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0f, v);

        // �Է��� ������ Idle�� ��ȯ
        if (moveDirection.magnitude < 0.1f)
        {
            player.ChangeState(new IdleState());
            return;
        }

        // ���� �Է��� ������ ���� ���·� ��ȯ
        if (Input.GetButtonDown("Fire1"))
        {
            player.ChangeState(new AttackState());
            return;
        }
    }

    public void UpdateState()
    {
        // �̵� �ִϸ��̼� ���� �� (��: blend tree ������Ʈ)
    }

    public void FixedUpdateState()
    {
        // Rigidbody�� ����� ���� ��� �̵�
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 movement = moveDirection.normalized * player.moveSpeed * Time.fixedDeltaTime;
            player.rb.MovePosition(player.rb.position + movement);

            // �̵� ���⿡ ���� ĳ���� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.rb.MoveRotation(Quaternion.Slerp(player.rb.rotation, targetRotation, 0.1f));
        }
    }
}
