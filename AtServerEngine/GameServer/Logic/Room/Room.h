////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"
#include "CoreMacro.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Object/ObjectTypes.h"
#include "Logic/Object/Actor/Player/PlayerTypes.h"
#include "Logic/Object/Actor/ActorTypes.h"
#include "Logic/Object/Actor/Monster/MonsterTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room class
////////////////////////////////////////////////////////////////////////////////////////////////////
class Room 
	: 
	public JobQueue
{
public:
	/// �ݹ��Լ� Ÿ�� ����
	using CallbackFunc = std::function< AtVoid() >;

	/// �÷��̾ �޴� �ݹ��Լ� Ÿ�� ����
	using CallbackPlayer = std::function< AtVoid( PlayerPtr ) >;

private:
	/// �� ��ȣ
	static std::atomic< AtInt32 > g_roomNum;

	/// �� ��ȣ
	AtInt32 m_roomNum;

protected:
	/// Lock
	USE_LOCK;

public:
	/// ������
	Room();

	/// �Ҹ���
	virtual ~Room();

	/// �÷��̾ �濡 �����Ų��.
	virtual AtBool HandleEnterPlayer( PlayerPtr player, CallbackFunc callback = nullptr );

	/// ������Ʈ�� ��ȯ�Ѵ�.
	virtual AtBool HandleSpawnObject( ObjectPtr object, CallbackFunc callback = nullptr );

	/// �÷��̾ �濡�� ��������.
	AtBool HandleLeavePlayer( PlayerPtr player, CallbackFunc callback = nullptr );

	/// �÷��̾��� �������� ó���Ѵ�.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

	/// �÷��̾���� ��ȸ�Ѵ�.
	AtVoid ForeachPlayer( CallbackPlayer callback, AtInt64 exceptId = 0 );

	/// �÷��̾�� ��ũ�Ѵ�.
	AtVoid SyncPlayers( PlayerPtr enterPlayer );

	/// �� �ѹ��� ��ȯ�Ѵ�.
	AtInt32 GetRoomNum() { return m_roomNum; }

public:
	/// Room��ü�� ��ȯ�Ѵ�.
	RoomPtr GetPtr();

	/// �� ��ȣ�� ��ȯ�Ѵ�.
	AtInt32 GetRoomNum() const;

	/// ���� ���� ��ȯ�Ѵ�.
	AtInt32 GetPlayerCount() const;

public:
	/// ���� ������Ʈ�Ѵ�.
	virtual AtVoid UpdateTick();

	/// ���� ��� �������� ��ε� ĳ���� �Ѵ�.
	AtVoid Broadcast( google::protobuf::Message& pkt, uint64 exceptId = 0 );

protected:
	/// ������Ʈ�� �߰��Ѵ�.
	AtBool _AddObject( ObjectPtr object );

	/// ������Ʈ�� �����Ѵ�.
	AtBool _RemoveObject( uint64 objectId );

protected:
	/// ��� ������Ʈ �����̳�
	unordered_map<uint64, ObjectPtr > m_objects;

	/// �÷��̾� �����̳�
	unordered_map<uint64, PlayerPtr > m_players;

	/// ���� �����̳�
	unordered_map<uint64, MonsterPtr > m_monsters;
};
