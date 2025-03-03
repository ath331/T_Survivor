////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"
#include "CoreMacro.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Object/Actor/Player/PlayerTypes.h"


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
	static std::atomic< AtInt32 > roomNum;

protected:
	/// Lock
	USE_LOCK;

public:
	/// ������
	Room();

	/// �Ҹ���
	virtual ~Room();

	/// ������Ʈ�� �����Ų��.
	AtBool EnterRoom( ObjectPtr object, AtBool randPos = true );

	/// ������Ʈ�� �����Ų��.
	AtBool LeaveRoom( ObjectPtr object );

	/// �÷��̾ �濡 �����Ų��.
	virtual AtBool HandleEnterPlayer( PlayerPtr player, CallbackFunc callback = nullptr );

	/// �÷��̾ �濡�� ��������.
	AtBool HandleLeavePlayer( PlayerPtr player );

	/// �÷��̾��� �������� ó���Ѵ�.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

	/// ä���� ��ε� ĳ���� �Ѵ�.
	AtVoid BroadcastChat( PlayerPtr sender, Protocol::C_Chat chat );

	/// �÷��̾���� ��ȸ�Ѵ�.
	AtVoid ForeachPlayer( CallbackPlayer callback, AtInt64 exceptId = 0 );

	/// �÷��̾�� ��ũ�Ѵ�.
	AtVoid SyncPlayers( PlayerPtr enterPlayer );

	/// �� �ѹ��� ��ȯ�Ѵ�.
	AtInt32 GetRoomNum() { return roomNum; }

public:
	/// Room��ü�� ��ȯ�Ѵ�.
	RoomPtr GetPtr();

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
	/// ��� ������Ʈ ��
	unordered_map<uint64, ObjectPtr > m_objects;

	/// �÷��̾� ��
	unordered_map<uint64, PlayerPtr > m_players;
};
