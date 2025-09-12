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

    public void Set_Animation(string animationType, EAnimationParamType paramType, bool boolValue)
    {
        if (animator != null)
        {
            switch (paramType)
            {
                case EAnimationParamType.AnimParamTypeBool:
                    animator.SetBool(animationType, boolValue);
                    break;
                // TODO : float 추가시 고쳐야함
                case EAnimationParamType.AnimParamTypeFloat:
                    animator.SetFloat(animationType, 0f);
                    break;

                case EAnimationParamType.AnimParamTypeTrigger:
                    animator.SetTrigger(animationType);
                    break;
            }
        }
    }
}
