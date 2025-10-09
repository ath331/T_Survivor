using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [Header("콤보 설정")]
    [SerializeField] private float comboResetTime = 1.2f; // 콤보 리셋 시간
    [SerializeField] private int maxComboCount = 3; // 이 무기는 3단 콤보

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