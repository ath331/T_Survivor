using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class AttackState : IPlayerState
{
    private PlayerController player;
    private float attackDuration = 0.5f; // 공격 애니메이션 길이 등
    private float timer;

    public void Enter(PlayerController player)
    {
        this.player = player;
        timer = 0;
        Debug.Log("Attack 상태 진입");

        // 공격 애니메이션 전환 및 무기 공격 호출
        if (player.weapon != null)
        {
            player.weapon.PerformAttack();
        }
    }

    public void Exit()
    {
        // 공격 상태 종료 후 처리 (애니메이션 정리 등)
    }

    public void HandleInput()
    {
        // 공격 중에는 입력 무시하거나 버퍼링할 수 있음
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;

        // 공격이 끝나면 Idle 또는 이동 상태로 복귀
        // 대충 예시로 timer attackDuration 적용 해놓았음. 
        // 공격 딜레이 같은건 따로 빼놓을 것 (ex. Weapon, PlayerController)
        if (timer >= attackDuration)
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState()
    {
        // 공격 중 물리 이동이 필요하다면 처리 (보통은 고정된 위치에서 공격)
    }
}
