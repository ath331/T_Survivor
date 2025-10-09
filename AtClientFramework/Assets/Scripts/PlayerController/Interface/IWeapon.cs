using UnityEngine;

public interface IWeapon
{
    // 무기가 장착될 때 호출되어, 소유자(PlayerController)의 정보를 받는다.
    void OnEquip(PlayerController playerController);

    // 무기가 해제될 때 호출.
    void OnUnequip();

    // 공격 버튼이 눌렸을 때 PlayerController로부터 호출될 메서드.
    void HandleAttackInput();

    void ComboOn();
    void ComboOff();
    void ComoboCounterReset();
    void QueueNextCombo();
    bool IsComboQueued { get; }
}