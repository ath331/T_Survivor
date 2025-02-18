using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class AttackState : IPlayerState
{
    private PlayerController player;
    private float attackDuration = 0.5f; // ���� �ִϸ��̼� ���� ��
    private float timer;

    public void Enter(PlayerController player)
    {
        this.player = player;
        timer = 0;
        Debug.Log("Attack ���� ����");

        // ���� �ִϸ��̼� ��ȯ �� ���� ���� ȣ��
        if (player.weapon != null)
        {
            player.weapon.PerformAttack();
        }
    }

    public void Exit()
    {
        // ���� ���� ���� �� ó�� (�ִϸ��̼� ���� ��)
    }

    public void HandleInput()
    {
        // ���� �߿��� �Է� �����ϰų� ���۸��� �� ����
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;

        // ������ ������ Idle �Ǵ� �̵� ���·� ����
        // ���� ���÷� timer attackDuration ���� �س�����. 
        // ���� ������ ������ ���� ������ �� (ex. Weapon, PlayerController)
        if (timer >= attackDuration)
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState()
    {
        // ���� �� ���� �̵��� �ʿ��ϴٸ� ó�� (������ ������ ��ġ���� ����)
    }
}
