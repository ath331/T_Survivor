using System.Collections.Generic;
using Protocol;
using UnityEngine;
using System;

public class PlayerListManager
{
    private static readonly PlayerListManager _instance = new PlayerListManager();
    public static PlayerListManager Instance => _instance;

    private Dictionary<ulong, PlayerController> _spawnedPlayers;

    private bool _isInitialized = false;

    public static event Action<Transform> OnLocalPlayerSpawned;

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

        if ( playerInfo.Id != 1000 ) 
        {
            // 랜덤 위치 생성
            Vector3 spawnPosition = new Vector3( playerInfo.PosInfo.X, playerInfo.PosInfo.Y, playerInfo.PosInfo.Z );

            // 오브젝트 풀에서 플레이어 가져오기
            GameObject playerObject = ObjectPoolManager.Instance.Get("Knight");
            playerObject.transform.position = spawnPosition;
            playerObject.SetActive( true );

            // 내 캐릭터인지 확인하고 IsLocalPlayer 활성화/비활성화
            PlayerController controller = playerObject.GetComponent<PlayerController>();
            controller.enabled = true;
            controller.networkPlayerTransform.enabled = true;
            controller.rb.useGravity = true;
            controller.rb.interpolation = RigidbodyInterpolation.None;

            controller.IsLocalPlayer = ( playerInfo.Id == MercuryHelper.mercuryId );

            // 카메라 등록
            if (controller.IsLocalPlayer)
            {
                // PlayerData(선택한 직업)에서 JobType을 가져옵니다.
                //JobType selectedJobType = _playerData.SelectedJobType;

                //// DataManager에서 해당 JobType의 데이터를 가져옵니다.
                //if (DataManager.Instance.JobDataTable.TryGetValue(selectedJobType, out JobData jobData))
                //{
                //    // 데이터를 기반으로 Job 인스턴스를 생성하여 설정합니다.
                //    IJob job = new Job(jobData);
                //    controller.SetJob(job);
                //}

                // TODO : 임시
                controller.EquipWeapon("Sword");

                // 내 캐릭터는 interpolation 적용
                controller.rb.interpolation = RigidbodyInterpolation.Interpolate;

                OnLocalPlayerSpawned?.Invoke(controller.transform);
            }

            // 생성된 플레이어 저장
            _spawnedPlayers[ playerInfo.Id ] = controller;
            Debug.Log( $"[NetworkManager] 플레이어 {playerInfo.Id} 스폰됨." );
        }

        //else if ( playerInfo.Id == 1000 )
        //{
        //    // 랜덤 위치 생성
        //    Vector3 spawnPosition = new Vector3( playerInfo.PosInfo.X, playerInfo.PosInfo.Y, playerInfo.PosInfo.Z );

        //    // 오브젝트 풀에서 플레이어 가져오기
        //    GameObject playerObject = ObjectPoolManager.Instance.Get( "TempMonster" );
        //    playerObject.transform.position = spawnPosition;
        //    playerObject.SetActive( true );

        //    // 내 캐릭터인지 확인하고 IsLocalPlayer 활성화/비활성화
        //    PlayerController controller = playerObject.GetComponent<PlayerController>();
        //    controller.IsLocalPlayer = false;

        //    // 생성된 플레이어 저장
        //    _spawnedPlayers[ playerInfo.Id ] = controller;
        //    Debug.Log( $"몬스터 스폰됨." );
        //}
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
