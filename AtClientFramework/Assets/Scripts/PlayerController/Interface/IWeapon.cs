using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �������̽� (�������� ������ ���)
public interface IWeapon
{
    string WeaponName { get; }
    void Attack();
}