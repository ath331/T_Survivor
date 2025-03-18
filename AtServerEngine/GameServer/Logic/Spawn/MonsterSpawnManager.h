////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif MonsterSpawnManager class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once


class Room;
class MonsterSpawnInfo;


class MonsterSpawnManager
{
public:
	/// ������
	MonsterSpawnManager( Room* room, AtInt32 spawnGroupId );

	/// �Ҹ���
	virtual ~MonsterSpawnManager();

	/// ������Ʈ
	AtVoid Update();

private:
	/// ��ȯ�� �� �ִ��� Ȯ���Ѵ�.
	AtBool _CheckSpawn( const MonsterSpawnInfo* spawnInfo );

	/// ��ȯ�Ѵ�.
	AtVoid _Spawn( const MonsterSpawnInfo* spawnInfo );

private:
	/// ��
	Room* m_room;

	/// ���� ���� ���� ���
	std::list< const MonsterSpawnInfo* > m_spawnInfoList;
};
