using UnityEngine;

// ��� ������ ������ �������̽�
public interface IEquipableItem
{
    string ItemName { get; }
    void Equip(PlayerController player);
}