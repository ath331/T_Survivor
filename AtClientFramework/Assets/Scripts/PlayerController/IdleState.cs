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
        // 이동 입력이 감지되면 이동 상태로 전환
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            player.ChangeState(new MoveState());
            return;
        }

        // 공격 버튼 입력(Fire1)이 있으면 공격 상태로 전환
        if (Input.GetButtonDown("Fire1"))
        {
            //player.ChangeState(new AttackState());
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
