using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class AttackState : IPlayerState
{
    private PlayerController player;

    public void Enter(PlayerController player)
    {
        this.player = player;
    }

    public void Exit()
    {
        var velocity = player.rb.velocity;

        player.rb.velocity = new Vector3(0, velocity.y, 0);
    }

    public void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player.Equipment.EquippedWeapon?.QueueNextCombo();
            return;
        }
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {
        player.Equipment.EquippedWeapon?.OnFixedUpdate();
    }
}
