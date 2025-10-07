using UnityEngine;
using Protocol;
using System.Collections.Generic;
using System.Linq;
public class StatController : MonoBehaviour
{
    public JobData BaseStats { get; private set; }
    private List<ItemData> _equippedItems = new List<ItemData>();

    public event System.Action OnStatsChanged;

    public void Initialize(JobData baseStats)
    {
        BaseStats = baseStats;
        OnStatsChanged?.Invoke();
    }

    public void EquipItem(ItemData itemData)
    {
        _equippedItems.Add(itemData);
        OnStatsChanged?.Invoke();
    }

    public void UnequipItem(ItemData itemData)
    {
        _equippedItems.Remove(itemData);
        OnStatsChanged?.Invoke();
    }

    // 최종 스탯을 계산하여 반환하는 메서드
    public int GetStat(EStat statType)
    {
        int finalStat = 0;
        // 1. 직업의 기본 스탯 가져오기
        switch (statType)
        {
            case EStat.StatHp: finalStat += BaseStats.HP; break;
            case EStat.StatMp: finalStat += BaseStats.MP; break;
                // ... 다른 기본 스탯들
        }

        // 2. 모든 장착 아이템의 스탯 보너스 합산
        finalStat += _equippedItems.Where(item => item.Stat == statType)
                                   .Sum(item => item.StatParam);

        return finalStat;
    }
}