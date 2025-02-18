using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // 무기 공격 로직을 담당
    public void PerformAttack()
    {
        // 예: 근접 공격이면 hit detection, 원거리면 projectile 발사 등
        Debug.Log("Weapon: 공격 수행");

        // 실제 구현에서는 애니메이션 이벤트와 연동하거나, 네트워크 호출 등 할 수 있음.
    }
}
