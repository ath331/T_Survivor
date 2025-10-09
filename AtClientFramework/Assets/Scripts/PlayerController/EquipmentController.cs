using UnityEngine;
using Protocol;
using System.Collections.Generic;
using static UnityEngine.UI.GridLayoutGroup;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] private Transform weaponHandSocket;

    private PlayerController _playerController;
    private StatController _statController;
    private Dictionary<EEquipSlotType, ItemData> _equippedItems = new Dictionary<EEquipSlotType, ItemData>();
    private Dictionary<EEquipSlotType, GameObject> _equippedItemObjects = new Dictionary<EEquipSlotType, GameObject>();

    public IWeapon EquippedWeapon { get; private set; }

    private void Awake()
    {
        _statController = GetComponent<StatController>();
        _playerController = GetComponent<PlayerController>();
    }

    public void EquipItem(ItemData newItem)
    {
        // 이미 해당 슬롯에 아이템이 있다면 해제
        if (_equippedItems.ContainsKey(newItem.EquipSlotType))
        {
            UnequipItem(newItem.EquipSlotType);
        }

        // 새 아이템 장착
        _equippedItems[newItem.EquipSlotType] = newItem;
        _statController.EquipItem(newItem); // StatController에 스탯 변경 알림

        // TODO : 일단 무기만 시각적으로 장착하고 IWeapon 인터페이스 가져오기
        if (newItem.EquipSlotType == EEquipSlotType.EquipSlotTypeWeapon)
        {
            GameObject weaponObject = ObjectPoolManager.Instance.Get(newItem.Name, weaponHandSocket);

            _equippedItemObjects[newItem.EquipSlotType] = weaponObject;

            EquippedWeapon = weaponObject.GetComponent<IWeapon>();

            EquippedWeapon.OnEquip(_playerController);

            if (EquippedWeapon is Sword sword)
            {
                _playerController.animator.runtimeAnimatorController = sword.AnimatorOverride;
            }
        }

        Debug.Log($"{newItem.Name} 장착 완료.");
    }

    public void UnequipItem(EEquipSlotType slot)
    {
        if (_equippedItems.TryGetValue(slot, out ItemData oldItem))
        {
            _statController.UnequipItem(oldItem);
            _equippedItems.Remove(slot);

            if (_equippedItemObjects.TryGetValue(slot, out GameObject itemObject))
            {
                ObjectPoolManager.Instance.Return(itemObject);

                _equippedItemObjects.Remove(slot);
            }

            if (slot == EEquipSlotType.EquipSlotTypeWeapon)
            {
                EquippedWeapon = null;
            }

            Debug.Log($"{oldItem.Name} 장착 해제.");
        }
    }
}