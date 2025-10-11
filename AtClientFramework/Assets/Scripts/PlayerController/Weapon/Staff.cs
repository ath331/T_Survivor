using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [Header("애니메이션")]
    [Tooltip("이 무기를 장착했을 때 적용할 Animator Override Controller")]
    [SerializeField] private AnimatorOverrideController animatorOverride;

    public bool IsComboQueued => _isComboQueued;

    private PlayerController _playerController;
    private bool _isComboQueued = false;

    public AnimatorOverrideController AnimatorOverride => animatorOverride;

    public void OnEquip(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void OnUnequip()
    {
        _playerController = null;
    }

    public void HandleAttackInput()
    {
        if (_playerController == null) return;

    }

    /// <summary>
    /// 스태프는 콤보공격 없음.
    /// </summary>
    public void QueueNextCombo()
    {
        _isComboQueued = false;
    }

    public void ComboOn()
    {

    }

    public void ComboOff()
    {

    }

    public void ComoboCounterReset()
    {

    }

    public void OnFixedUpdate()
    {

    }
}