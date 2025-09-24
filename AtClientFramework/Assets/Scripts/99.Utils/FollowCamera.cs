using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 12, -6);
    [SerializeField] private float smoothSpeed = 0.125f;

    private Transform target; // 따라다닐 타겟 (로컬 플레이어)

    private void OnEnable()
    {
        PlayerListManager.OnLocalPlayerSpawned += SetTarget;
    }

    private void OnDisable()
    {
        PlayerListManager.OnLocalPlayerSpawned -= SetTarget;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Debug.Log("카메라 타겟 설정 완료: " + newTarget.name);
    }
}