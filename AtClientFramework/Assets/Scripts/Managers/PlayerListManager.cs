using System.Collections.Generic;
using Protocol;
using UnityEngine;
using System;

public class PlayerListManager : SingletonMonoBehaviour<PlayerListManager>
{
    private Dictionary<ulong, PlayerController> _spawnedPlayers;

    private bool _isInitialized = false;

    public static Action<Transform> OnLocalPlayerSpawned;

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _spawnedPlayers = new Dictionary<ulong, PlayerController>();

        _isInitialized = true;
    }

    public void RegisterPlayer(ulong playerId, PlayerController controller)
    {
        if (!_spawnedPlayers.ContainsKey(playerId))
        {
            _spawnedPlayers[playerId] = controller;
            Debug.Log($"[PlayerListManager] 플레이어 {playerId} 등록됨.");
        }
    }

    public void UnregisterPlayer(ulong playerId)
    {
        if (_spawnedPlayers.TryGetValue(playerId, out PlayerController controller))
        {
            ObjectPoolManager.Instance.Return(controller.gameObject);
            _spawnedPlayers.Remove(playerId);
            Debug.Log($"[PlayerListManager] 플레이어 {playerId} 등록 해제됨.");
        }
    }

    public bool TryGetPlayer(ulong playerId, out PlayerController player)
    {
        return _spawnedPlayers.TryGetValue(playerId, out player);
    }
}
