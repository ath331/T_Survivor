using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    public WeaponType GetWeaponType() => WeaponType.SWORD;

    public void Attack(PlayerController attacker)
    {
        attacker.animator.SetTrigger("Attack");

        // 애니메이션 이벤트(OnAttackHit)를 통해 데미지 판정을 활성화.
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    WeaponType IWeapon.GetWeaponType()
    {
        throw new System.NotImplementedException();
    }
}