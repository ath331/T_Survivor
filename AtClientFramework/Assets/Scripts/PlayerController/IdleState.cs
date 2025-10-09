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
        // 상태 종료 시 처리할 내용 (필요하면 작성)
        Debug.Log("Player Idle Exit");
    }

    public void HandleInput()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            player.ChangeState(player.moveState);
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            player.Equipment.EquippedWeapon?.HandleAttackInput();
            return;
        }
    }

    public void UpdateState()
    {
        // Idle 상태일 때 업데이트할 로직 (예: 가만히 있을 때 애니메이션 유지)
    }

    public void FixedUpdateState()
    {
        // 물리 이동 없음
    }
}
