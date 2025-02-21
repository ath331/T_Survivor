using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController player;

    public void Enter(PlayerController player)
    {
        Debug.Log("Player Idle Enter");

        this.player = player;
    }

    public void Exit()
    {
        // ���� ���� �� ó���� ���� (�ʿ��ϸ� �ۼ�)
        Debug.Log("Player Idle Exit");
    }

    public void HandleInput()
    {
        // �̵� �Է��� �����Ǹ� �̵� ���·� ��ȯ
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            player.ChangeState(new MoveState());
            return;
        }

        // ���� ��ư �Է�(Fire1)�� ������ ���� ���·� ��ȯ
        if (Input.GetButtonDown("Fire1"))
        {
            //player.ChangeState(new AttackState());
            return;
        }
    }

    public void UpdateState()
    {
        // Idle ������ �� ������Ʈ�� ���� (��: ������ ���� �� �ִϸ��̼� ����)
    }

    public void FixedUpdateState()
    {
        // ���� �̵� ����
    }
}
