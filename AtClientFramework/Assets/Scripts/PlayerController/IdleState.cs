using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController player;

    public void Enter(PlayerController player)
    {
        this.player = player;
        // Idle �ִϸ��̼� ��ȯ �� �ʱ�ȭ �۾�
        Debug.Log("Player Idle Enter");
    }

    public void Exit()
    {
        // ���� ���� �� ó���� ���� (�ʿ��ϸ� �ۼ�)
        Debug.Log("Player Idle Exit");
    }

    public void HandleInput()
    {
        // �̵� �Է��� �����Ǹ� �̵� ���·� ��ȯ
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            player.ChangeState(new MoveState());
            return;
        }

        // ���� ��ư �Է�(Fire1)�� ������ ���� ���·� ��ȯ
        if (Input.GetButtonDown("Fire1"))
        {
            player.ChangeState(new AttackState());
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
