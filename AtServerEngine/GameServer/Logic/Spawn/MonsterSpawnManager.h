////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif MonsterSpawnManager class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once


class Room;
class MonsterSpawnInfo;


class MonsterSpawnManager
{
public:
	/// 생성자
	MonsterSpawnManager( Room* room, AtInt32 spawnGroupId );

	/// 소멸자
	virtual ~MonsterSpawnManager();

	/// 업데이트
	AtVoid Update();

private:
	/// 소환할 수 있는지 확인한다.
	AtBool _CheckSpawn( const MonsterSpawnInfo* spawnInfo );

	/// 소환한다.
	AtVoid _Spawn( const MonsterSpawnInfo* spawnInfo );

private:
	/// 룸
	Room* m_room;

	/// 스폰 몬스터 정보 목록
	std::list< const MonsterSpawnInfo* > m_spawnInfoList;
};
