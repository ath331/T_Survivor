using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }

    [Header("정보 설정")]
    public WeaponController weapon; // 일단 대충.. 나중에 지울것
    public IJob CurrentJob { get; private set; }
    public IWeapon EquippedWeapon { get; private set; }
    public List<Skill> Skills { get; private set; } = new List<Skill>();

    public bool IsLocalPlayer { get; set; } // 내 캐릭터 여부 (NetworkManager에서 설정)

    // 현재 상태 (초기에는 Idle 상태)
    private IPlayerState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 초기 상태를 IdleState로 설정
        ChangeState(new IdleState());
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            // input update
            currentState.HandleInput(); 
        }

        // state update
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        // physics update
        currentState.FixedUpdateState();
    }

    /// <summary>
    /// 상태 전환 메서드
    /// </summary>
    public void ChangeState(IPlayerState newState)
    {
        // 현재 state Exit 
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        // 다음 state Enter
        if (currentState != null)
            currentState.Enter(this);
    }
}
