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

    }
}
