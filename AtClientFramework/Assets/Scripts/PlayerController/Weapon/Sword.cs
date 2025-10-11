using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [Header("콤보 설정")]
    [SerializeField] private int maxComboCount = 3; // 이 무기는 3단 콤보
    [SerializeField] private float attackMoveSpeed = 10f;

    [Header("애니메이션")]
    [Tooltip("이 무기를 장착했을 때 적용할 Animator Override Controller")]
    [SerializeField] private AnimatorOverrideController animatorOverride;

    private PlayerController _playerController;
    private int _comboCounter = 0;
    private float _lastAttackTime = 0f;

    private bool _isComboWindowOpen = false; // "콤보 창"이 열려있는지 여부
    private bool _isComboQueued = false;     // 다음 콤보가 '예약'되었는지 여부

    public bool IsComboQueued => _isComboQueued;
    public AnimatorOverrideController AnimatorOverride => animatorOverride;

    public void OnEquip(PlayerController playerController)
    {
        _playerController = playerController;
        _comboCounter = 0; // 장착 시 콤보 카운터 초기화
    }

    public void OnUnequip()
    {
        _playerController = null;
    }

    public void HandleAttackInput()
    {
        if (_playerController == null) return;

        _isComboQueued = false;

        _comboCounter = (_comboCounter % maxComboCount) + 1;

        _playerController.animator.SetInteger("AttackIndex", _comboCounter);

        _playerController.ChangeState(_playerController.attackState);
    }

    public void OnFixedUpdate()
    {
        if (_playerController == null) return;

        // 1. 애니메이터에서 현재 프레임의 AttackMovement 커브 값을 읽어옵니다.
        float moveMultiplier = _playerController.animator.GetFloat("AttackMovement");

        // 2. 이 값을 전진 속도로 변환하여 Rigidbody의 속도를 설정합니다.
        Vector3 attackVelocity = _playerController.transform.forward * moveMultiplier * attackMoveSpeed;

        // y축 속도는 중력 등을 위해 기존 값을 유지합니다.
        attackVelocity.y = _playerController.rb.velocity.y;

        _playerController.rb.velocity = attackVelocity;
    }

    public void QueueNextCombo()
    {
        // 콤보 창이 열려있을 때만 콤보를 예약.
        if (_isComboWindowOpen)
        {
            _isComboQueued = true;
        }
    }

    public void ComboOn()
    {
        _isComboWindowOpen = true;
    }
    
    public void ComboOff()
    {
        _isComboWindowOpen = false;
    }

    public void ComoboCounterReset()
    {
        _comboCounter = 0;
    }
}