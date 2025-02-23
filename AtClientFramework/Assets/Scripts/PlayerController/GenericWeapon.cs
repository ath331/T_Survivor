using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeapon : IWeapon
{
    private ItemData itemData;
    public string WeaponName => itemData.itemName;

    public GenericWeapon(ItemData data)
    {
        itemData = data;
    }

    public void Attack()
    {
        Debug.Log($"Attacking with weapon: {WeaponName} with bonus attack {itemData.bonusAttack}");
        // �߰� ���� ���� �� ����Ʈ ó�� ����
    }
}
