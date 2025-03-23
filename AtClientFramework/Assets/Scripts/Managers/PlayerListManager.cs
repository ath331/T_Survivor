using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class PlayerListManager
{
    private static readonly PlayerListManager _instance = new PlayerListManager();
    public static PlayerListManager Instance => _instance;

    private Dictionary<ulong, PlayerController> _spawnedPlayers;

    private bool _isInitialized = false;


    public void Initialize()
    {
        if (_isInitialized)
            return;

        _spawnedPlayers = new Dictionary<ulong, PlayerController>();

        _isInitialized = true;
    }

    public void ProcessSpawnHandler(ObjectInfo playerInfo)
    {
        // 이미 스폰된 플레이어라면 무시
        if (_spawnedPlayers.ContainsKey(playerInfo.Id))
            return;

        // 랜덤 위치 생성
        Vector3 spawnPosition = new Vector3(playerInfo.PosInfo.X, playerInfo.PosInfo.Y, playerInfo.PosInfo.Z);

        // 오브젝트 풀에서 플레이어 가져오기
        GameObject playerObject = ObjectPoolManager.Instance.Get("Character");
        playerObject.transform.position = spawnPosition;
        playerObject.SetActive(true);

        // 내 캐릭터인지 확인하고 IsLocalPlayer 활성화/비활성화
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.IsLocalPlayer = (playerInfo.Id == MercuryHelper.mercuryId);
        controller.Send_Move();

        // 생성된 플레이어 저장
        _spawnedPlayers[playerInfo.Id] = controller;
        Debug.Log($"[NetworkManager] 플레이어 {playerInfo.Id} 스폰됨.");
    }

    /// <summary> 플레이어 제거 (나갔을 때) </summary>
    public void RemovePlayer(ulong playerId)
    {
        if (_spawnedPlayers.ContainsKey(playerId))
        {
            PlayerController playerObject = _spawnedPlayers[playerId];
            ObjectPoolManager.Instance.Return(playerObject.gameObject); // 오브젝트 풀로 반환
            _spawnedPlayers.Remove(playerId);
            Debug.Log($"[NetworkManager] 플레이어 {playerId} 제거됨.");
        }
    }

    public bool TryGetPlayer(ulong playerId, out PlayerController player)
    {
        return _spawnedPlayers.TryGetValue(playerId, out player);
    }
}
