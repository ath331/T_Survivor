using UnityEngine;
using Protocol;
using System.Collections.Generic;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] private Transform weaponHandSocket;

    private StatController _statController;
    private Dictionary<EEquipSlotType, ItemData> _equippedItems = new Dictionary<EEquipSlotType, ItemData>();
    private Dictionary<EEquipSlotType, GameObject> _equippedItemObjects = new Dictionary<EEquipSlotType, GameObject>();

    public IWeapon EquippedWeapon { get; private set; }

    private void Awake()
    {
        _statController = GetComponent<StatController>();
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

        // 무기일 경우, 시각적으로 장착하고 IWeapon 인터페이스 가져오기
        if (newItem.EquipSlotType == EEquipSlotType.EquipSlotTypeWeapon)
        {
            // TODO: ItemData에 프리팹 경로/이름 정보가 있어야 함
            GameObject weaponPrefab = Resources.Load<GameObject>($"Weapons/{newItem.Name}");
            GameObject weaponObject = Instantiate(weaponPrefab, weaponHandSocket);
            _equippedItemObjects[newItem.EquipSlotType] = weaponObject;
            EquippedWeapon = weaponObject.GetComponent<IWeapon>();
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
                Destroy(itemObject);
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