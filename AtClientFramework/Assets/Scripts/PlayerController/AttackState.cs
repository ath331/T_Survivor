using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class AttackState : IPlayerState
{
    private PlayerController player;

    public void Enter(PlayerController player)
    {
        this.player = player;

        player.animator.SetTrigger("Attack");
    }

    public void Exit()
    {
    }

    public void HandleInput()
    {

    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {

    }
}
