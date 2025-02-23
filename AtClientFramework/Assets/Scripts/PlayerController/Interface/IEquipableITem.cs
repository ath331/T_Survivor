using UnityEngine;

// 장비 가능한 아이템 인터페이스
public interface IEquipableItem
{
    string ItemName { get; }
    void Equip(PlayerController player);
}