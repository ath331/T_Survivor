using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;

    public void Attack(PlayerController attacker)
    {
        attacker.animator.SetTrigger("CastSpell");
    }

    WeaponType IWeapon.GetWeaponType()
    {
        throw new System.NotImplementedException();
    }
}