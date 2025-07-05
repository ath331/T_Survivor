////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif MonsterSpawnManager class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "MonsterSpawnManager.h"
#include "Logic/Room/Room.h"
#include "Logic/Object/Actor/Monster/Monster.h"
#include "Logic/Object/Actor/Monster/MonsterTypes.h"
#include "Data/Spawn/MonsterSpawnInfoManager.h"
#include "Data/Monster/MonsterInfoManager.h"
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
MonsterSpawnManager::MonsterSpawnManager( Room* room, AtInt32 spawnGroupId )
	:
	m_room( room )
{
	// TODO : 스폰 그룹 아이디로 리스트를 가져와야하는데 아직 데이터 툴이 안 되어있어서 임시로 하나씩 생성
	{
		m_spawnInfoList.push_back( MonsterSpawnInfoManager::GetInstance().GetInfo( 1 ) );
		m_spawnInfoList.push_back( MonsterSpawnInfoManager::GetInstance().GetInfo( 2 ) );
		m_spawnInfoList.push_back( MonsterSpawnInfoManager::GetInstance().GetInfo( 3 ) );
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소멸자
////////////////////////////////////////////////////////////////////////////////////////////////////
MonsterSpawnManager::~MonsterSpawnManager()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 업데이트
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid MonsterSpawnManager::Update()
{
	// INFO_LOG( "MonsterSpawnTick" );

	for ( const auto spawnInfo : m_spawnInfoList )
	{
		if ( !spawnInfo )
			continue;

		if ( _CheckSpawn( spawnInfo ) )
			_Spawn( spawnInfo );
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소환할 수 있는지 확인한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool MonsterSpawnManager::_CheckSpawn( const MonsterSpawnInfo* spawnInfo )
{
	if ( !spawnInfo )
		return false;

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid MonsterSpawnManager::_Spawn( const MonsterSpawnInfo* spawnInfo )
{
	if ( !spawnInfo )
		return;

	MonsterPtr newMonster = std::make_shared< Monster >( 
		spawnInfo->GetMonsterInfoId(),
		spawnInfo->GetAIInfoId() );

	newMonster->objectInfo->set_id         ( 1000 ); // TODO : 머큐리 생성기 필요
	newMonster->objectInfo->set_infoid     ( spawnInfo->GetMonsterInfoId() );
	newMonster->objectInfo->set_object_type( Protocol::OBJECT_TYPE_ACTOR   );
	newMonster->objectInfo->set_actor_type ( Protocol::ACTOR_TYPE_MONSTER  );
	newMonster->posInfo->set_x( Utils::GetRandom( -15.f, 15.f ) ); // 좌표는 외부 데이터로?
	newMonster->posInfo->set_z( Utils::GetRandom( -15.f, 15.f ) ); // 좌표는 외부 데이터로?
	newMonster->posInfo->set_y( 0.0f );

	if ( m_room )
	{
		m_room->DoAsync( 
			&Room::HandleSpawnObject, 
			newMonster->shared_from_this(), 
			(Room::CallbackFunc)( []() {} ) );
	}
}
