using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;

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

    private ulong mercuryId = 0;

    // 현재 상태 (초기에는 Idle 상태)
    private IPlayerState currentState;

    private Vector3 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 초기 상태를 IdleState로 설정
        ChangeState(new IdleState());

        mercuryId = MercuryHelper.mercuryId;

        Send_Move(transform.position);
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

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }

    public void Send_Move(Vector3 pos)
    {
        if (!IsLocalPlayer) return;

        C_Move pkt = new C_Move
        {
            Info = new PosInfo
            {
                Id = mercuryId,

                X = pos.x,
                Y = pos.y,
                Z = pos.z,
            }
        };

        NetworkManager.Instance.Send(pkt);
    }

    /// <summary>
    /// 네트워크에서 받은 위치 업데이트 (다른 플레이어 전용)
    /// </summary>
    public void UpdateNetworkPosition(Vector3 newPosition)
    {
        if (!IsLocalPlayer)
        {
            targetPosition = newPosition;
        }
    }

    /// <summary>
    /// 다른 플레이어의 위치를 서버 데이터로 부드럽게 보간 이동
    /// </summary>
    public void SyncWithNetwork()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }
}
