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

    public void SetTarget(float x, float y, float z, float yaw)
    {
        targetPosition.Set(x, y, z);

        targetRotation = Quaternion.Euler(0, yaw, 0);
    }

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
