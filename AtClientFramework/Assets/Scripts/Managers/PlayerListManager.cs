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
        // �̹� ������ �÷��̾��� ����
        if (_spawnedPlayers.ContainsKey(playerInfo.Id))
            return;

        // ���� ��ġ ����
        Vector3 spawnPosition = new Vector3(playerInfo.PosInfo.X, playerInfo.PosInfo.Y, playerInfo.PosInfo.Z);

        // ������Ʈ Ǯ���� �÷��̾� ��������
        GameObject playerObject = ObjectPoolManager.Instance.Get("Character");
        playerObject.transform.position = spawnPosition;
        playerObject.SetActive(true);

        // �� ĳ�������� Ȯ���ϰ� IsLocalPlayer Ȱ��ȭ/��Ȱ��ȭ
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.IsLocalPlayer = (playerInfo.Id == MercuryHelper.mercuryId);
        controller.Send_Move();

        // ������ �÷��̾� ����
        _spawnedPlayers[playerInfo.Id] = controller;
        Debug.Log($"[NetworkManager] �÷��̾� {playerInfo.Id} ������.");
    }

    /// <summary> �÷��̾� ���� (������ ��) </summary>
    public void RemovePlayer(ulong playerId)
    {
        if (_spawnedPlayers.ContainsKey(playerId))
        {
            PlayerController playerObject = _spawnedPlayers[playerId];
            ObjectPoolManager.Instance.Return(playerObject.gameObject); // ������Ʈ Ǯ�� ��ȯ
            _spawnedPlayers.Remove(playerId);
            Debug.Log($"[NetworkManager] �÷��̾� {playerId} ���ŵ�.");
        }
    }

    public bool TryGetPlayer(ulong playerId, out PlayerController player)
    {
        return _spawnedPlayers.TryGetValue(playerId, out player);
    }
}
