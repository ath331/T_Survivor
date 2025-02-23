using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 인터페이스 (아이템이 무기일 경우)
public interface IWeapon
{
    string WeaponName { get; }
    void Attack();
}