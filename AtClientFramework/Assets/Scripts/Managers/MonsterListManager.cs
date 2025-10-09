using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// TODO : PlayerController를 임시로 사용중 -> MonsterController 새로 만들어야함!
/// </summary>
public class MonsterListManager : SingletonMonoBehaviour<MonsterListManager>
{
    private Dictionary<ulong, PlayerController> _spawnedMonsters;
    private bool _isInitialized = false;

    // 몬스터가 스폰되거나 죽었을 때 알림을 보낼 이벤트 (필요 시 사용)
    public event Action<PlayerController> OnMonsterSpawned;
    public event Action<ulong> OnMonsterDespawned;

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _spawnedMonsters = new Dictionary<ulong, PlayerController>();
        _isInitialized = true;
    }

    public void RegisterMonster(ulong monsterId, PlayerController controller)
    {
        if (!_spawnedMonsters.ContainsKey(monsterId))
        {
            _spawnedMonsters[monsterId] = controller;
            OnMonsterSpawned?.Invoke(controller);
            Debug.Log($"[MonsterListManager] 몬스터 {monsterId} 등록됨.");
        }
    }

    public void UnregisterMonster(ulong monsterId)
    {
        if (_spawnedMonsters.TryGetValue(monsterId, out PlayerController controller))
        {
            ObjectPoolManager.Instance.Return(controller.gameObject);
            _spawnedMonsters.Remove(monsterId);
            OnMonsterDespawned?.Invoke(monsterId);
            Debug.Log($"[MonsterListManager] 몬스터 {monsterId} 등록 해제됨.");
        }
    }

    public bool TryGetMonster(ulong monsterId, out PlayerController monster)
    {
        return _spawnedMonsters.TryGetValue(monsterId, out monster);
    }
}