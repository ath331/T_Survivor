using System.Collections;
using System.Collections.Generic;
using Protocol;
using TMPro;
using UnityEngine;

public class NetworkPlayerAnimation : MonoBehaviour
{
    public Animator animator { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Bool 타입 파라미터 설정
    public void SetAnimation(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    // Float 타입 파라미터 설정
    public void SetAnimation(string parameterName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }

    // Trigger 타입 파라미터 설정
    public void SetTrigger(string parameterName)
    {
        if (animator != null)
        {
            animator.SetTrigger(parameterName);
        }
    }
}
