using UnityEngine;
using Protocol;

public class MoveState : IPlayerState
{
    private PlayerController player;
    private Vector3 moveDirection;
    private float moveSpeed = 5f;
    private float rotationSpeed = 20f;
    
    private float sendTimer;
    private readonly float sendInterval = 0.1f;

    public void Enter(PlayerController player)
    {
        this.player = player;
        moveDirection = Vector3.zero;

        player.Send_Anim(EAnimationParamType.AnimParamTypeBool, "IsMoving", true);

        player.Send_Move();
    }

    public void Exit()
    {
        moveDirection = Vector3.zero;
        player.rb.velocity = Vector3.zero;

        player.Send_Anim(EAnimationParamType.AnimParamTypeBool, "IsMoving", false);

        player.Send_Move();
    }
    public void HandleInput()
    {
        if (!player.IsLocalPlayer) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(h, 0f, v).normalized;

        if (moveDirection == Vector3.zero)
        {
            player.ChangeState(player.idleState);
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
        sendTimer += Time.deltaTime;
        if (sendTimer >= sendInterval)
        {
            sendTimer = 0f;
            player.Send_Move();
        }
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
        }
        else
        {
            player.rb.velocity = Vector3.zero;
        }
    }
}
