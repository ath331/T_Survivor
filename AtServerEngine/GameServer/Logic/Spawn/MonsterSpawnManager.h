////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif MonsterSpawnManager class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once


class PlayRoom;
class MonsterSpawnInfo;


class MonsterSpawnManager
{
public:
	/// 생성자
	MonsterSpawnManager( PlayRoom* room, AtInt32 spawnGroupId );

	/// 소멸자
	virtual ~MonsterSpawnManager();

	/// 업데이트
	AtVoid Update( Millisecond curTime );

private:
	/// 소환할 수 있는지 확인한다.
	AtBool _CheckSpawn( const MonsterSpawnInfo* spawnInfo );

	/// 소환한다.
	AtVoid _Spawn( const MonsterSpawnInfo* spawnInfo );

private:
	/// 룸
	PlayRoom* m_room;

	/// 스폰 몬스터 정보 목록
	std::list< const MonsterSpawnInfo* > m_spawnInfoList;

	/// 반복 소환 임시 막기
	set< AtInt32 > m_tempDuplicateSpawnSet;
};
