using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Rigidbody))]
public class NetworkPlayerTransform : MonoBehaviour
{
    [Header("보간 설정")]
    [SerializeField] float positionLerpSpeed = 10f;
    [SerializeField] float rotationLerpSpeed = 20f;

    PlayerController playerController;
    Rigidbody rb;

    Vector3 targetPosition;

    Quaternion targetRotation;
    
    // 보간에 사용할 스냅샷을 저장하는 버퍼
    private readonly List<Snapshot> _snapshotBuffer = new List<Snapshot>();

    // 보간을 위한 시간 지연. 패킷 전송 간격(sendInterval)의 2배 정도로 설정하는 것이 일반적
    private const float INTERPOLATION_BUFFER_TIME = 0.2f;

    // 스냅샷 데이터 구조체
    private struct Snapshot
    {
        public float timestamp;
        public Vector3 position;
        public Quaternion rotation;

        public Snapshot(float timestamp, Vector3 position, Quaternion rotation)
        {
            this.timestamp = timestamp;
            this.position = position;
            this.rotation = rotation;
        }
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyInterpolation();
    }

    public void SetTarget(Vector3 pos, float yaw)
    {
        targetPosition = pos;
        targetRotation = Quaternion.Euler(0, yaw, 0);
    }

    //[보간 버퍼링]
    // 핵심 아이디어: "가장 최근에 받은 위치" 하나만을 향해 달려가는 대신,
    //"이전 위치 정보"와 "최신 위치 정보" 두 개를 모두 기억하고,
    // 그 "사이" 를 부드럽게 채워주는 방식.
    private void ApplyInterpolation()
    {
        if (playerController.IsLocalPlayer) return;

        // Rigidbody 보간 이동
        if (rb != null)
        {
            float positionStep = positionLerpSpeed * Time.fixedDeltaTime;
            rb.MovePosition(Vector3.MoveTowards(
                rb.position,
                targetPosition,
                positionStep));

            float rotationStep = rotationLerpSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationStep));

            //rb.MovePosition(Vector3.Lerp(
            //    rb.position,
            //    targetPosition,
            //    positionLerpSpeed * Time.fixedDeltaTime));

            //rb.MoveRotation(Quaternion.Slerp(
            //    rb.rotation,
            //    targetRotation,
            //    rotationLerpSpeed * Time.fixedDeltaTime));
        }
    }
}
