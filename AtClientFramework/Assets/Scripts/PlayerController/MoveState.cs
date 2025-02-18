using UnityEngine;

public class MoveState : IPlayerState
{
    private PlayerController player;
    private Vector3 moveDirection;

    public void Enter(PlayerController player)
    {
        this.player = player;
        Debug.Log("Move 상태 진입");
    }

    public void Exit()
    {
        // 상태 종료 시 입력 초기화 등
        moveDirection = Vector3.zero;
    }

    public void HandleInput()
    {
        // WASD 혹은 조이스틱 입력을 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0f, v);

        // 입력이 없으면 Idle로 전환
        if (moveDirection.magnitude < 0.1f)
        {
            player.ChangeState(new IdleState());
            return;
        }

        // 공격 입력이 들어오면 공격 상태로 전환
        if (Input.GetButtonDown("Fire1"))
        {
            player.ChangeState(new AttackState());
            return;
        }
    }

    public void UpdateState()
    {
        // 이동 애니메이션 갱신 등 (예: blend tree 업데이트)
    }

    public void FixedUpdateState()
    {
        // Rigidbody를 사용해 물리 기반 이동
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 movement = moveDirection.normalized * player.moveSpeed * Time.fixedDeltaTime;
            player.rb.MovePosition(player.rb.position + movement);

            // 이동 방향에 따라 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.rb.MoveRotation(Quaternion.Slerp(player.rb.rotation, targetRotation, 0.1f));
        }
    }
}
