using UnityEngine;
using Protocol;
using UnityEditor.PackageManager;

public class SpawnManager : SingletonMonoBehaviour<SpawnManager>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private CharacterDatabase characterDatabase;


    public void ProcessSpawn(ObjectInfo actorInfo)
    {
        switch (actorInfo.ActorType)
        {
            case EActorType.ActorTypePlayer:
                SpawnPlayer(actorInfo);
                break;
            case EActorType.ActorTypeNone:
                SpawnPlayer(actorInfo);
                break;
            case EActorType.ActorTypeMonster:
                SpawnMonster(actorInfo);
                break;
        }
    }

    public void SpawnPlayer(ObjectInfo playerInfo)
    {
        // 이미 생성된 플레이어인지 PlayerListManager에 확인
        if (PlayerListManager.Instance.TryGetPlayer(playerInfo.Id, out _))
        {
            return;
        }

        bool isLocal = (playerInfo.Id == MercuryHelper.mercuryId);
        CharacterData characterToSpawn = null;

        if (isLocal)
        {
            // TODO : 임시 캐릭터 선택
            CharacterData knightData = characterDatabase.GetCharacter(EPlayerType.PlayerTypeKnight);
            playerData.SetCharacter(knightData);

            // 로컬 플레이어: PlayerData에서 선택한 캐릭터 정보를 가져옴
            characterToSpawn = playerData.SelectedCharacter;
        }
        else
        {
            // 다른 플레이어: playerInfo에 담겨온 직업 정보로 Database에서 찾음
            EPlayerType remoteJobType = EPlayerType.PlayerTypeKnight/*(EPlayerType)playerInfo.JobType*/;
            characterToSpawn = characterDatabase.GetCharacter(remoteJobType);
        }

        if (characterToSpawn == null)
        {
            Debug.LogError($"{playerInfo.Id}에 해당하는 캐릭터 데이터를 찾을 수 없습니다.");
            return;
        }

        // 실제 프리팹 스폰
        GameObject playerObject = ObjectPoolManager.Instance.Get(characterToSpawn.prefabName);
        playerObject.transform.position = new Vector3(playerInfo.PosInfo.X, playerInfo.PosInfo.Y, playerInfo.PosInfo.Z);

        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.IsLocalPlayer = isLocal;

        // 데이터와 컴포넌트 초기화
        InitializeController(controller, characterToSpawn.jobType);

        // 플레이어 리스트 목록에 등록
        PlayerListManager.Instance.RegisterPlayer(playerInfo.Id, controller);

        if (isLocal)
        {
            PlayerListManager.OnLocalPlayerSpawned?.Invoke(controller.transform);
        }
    }

    public void SpawnMonster(ObjectInfo monsterInfo)
    {
        GameObject playerObject = ObjectPoolManager.Instance.Get("TempMonster");
        playerObject.transform.position = new Vector3(monsterInfo.PosInfo.X, monsterInfo.PosInfo.Y, monsterInfo.PosInfo.Z);
        
        // PlayerController 임시.
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.IsLocalPlayer = false;

        MonsterListManager.Instance.RegisterMonster(monsterInfo.Id, controller);
    }

    private void InitializeController(PlayerController controller, EPlayerType jobType)
    {
        // DataManager에서 직업 기본 스탯과 기본 무기 정보를 가져와서 초기화
        if (DataLoader.Instance.JobDataTable.TryGetValue(jobType, out JobData jobData))
        {
            // TODO : 임시 Sword 가져오기
            int defaultWeaponId = 1000; 
            ItemData defaultWeapon = DataLoader.Instance.ItemDataTable[defaultWeaponId];

            controller.Initialize(jobData, defaultWeapon);
        }

        // 컴포넌트 활성화
        controller.enabled = true;
        controller.networkPlayerTransform.enabled = true;
        controller.rb.useGravity = true;
        controller.rb.interpolation = controller.IsLocalPlayer ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
    }
}