using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MoveState : IPlayerState
{
    private PlayerController player;
    private Vector3 moveDirection;
    private float moveSpeed = 5f;
    private float rotationSpeed = 20f;

    public void Enter(PlayerController player)
    {
        Debug.Log("Entered Move State");
        this.player = player;

        moveDirection = Vector3.zero;

        player.animator.SetBool("IsMoving", true);
    }

    public void Exit()
    {
        Debug.Log("Exit Move State");
        moveDirection = Vector3.zero;
        player.rb.velocity = Vector3.zero;
        player.animator.SetBool("IsMoving", false);
    }
    public void HandleInput()
    {
        if (!player.IsLocalPlayer) return; // 내 캐릭터만 입력을 받음

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(h, 0f, v).normalized;

        if (moveDirection == Vector3.zero)
        {
            player.ChangeState(new IdleState());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            return;
        }
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {
        // 내 캐릭터는 직접 이동
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 targetDirection = moveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.rb.MoveRotation(Quaternion.Slerp(player.rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));

            Vector3 velocity = targetDirection * moveSpeed;
            player.rb.velocity = new Vector3(velocity.x, player.rb.velocity.y, velocity.z);

            player.Send_Move();
        }
        else
        {
            player.rb.velocity = Vector3.zero;
        }
    }
}
