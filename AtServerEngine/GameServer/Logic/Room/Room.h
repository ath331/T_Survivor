////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Object/Actor/Player/PlayerTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room class
////////////////////////////////////////////////////////////////////////////////////////////////////
class Room 
	: 
	public JobQueue
{
private:
	/// �� ��ȣ
	static std::atomic< AtInt32 > roomNum;

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
	virtual AtBool HandleEnterPlayer( PlayerPtr player );

	/// �÷��̾ �濡�� ��������.
	AtBool HandleLeavePlayer( PlayerPtr player );

	/// �÷��̾��� �������� ó���Ѵ�.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

	/// ä���� ��ε� ĳ���� �Ѵ�.
	AtVoid BroadcastChat( PlayerPtr sender, Protocol::C_Chat chat );

public:
	/// Room��ü�� ��ȯ�Ѵ�.
	RoomPtr GetPtr();

public:
	/// ���� ������Ʈ�Ѵ�.
	virtual AtVoid UpdateTick();

protected:
	/// ������Ʈ�� �߰��Ѵ�.
	AtBool _AddObject( ObjectPtr object );

	/// ������Ʈ�� �����Ѵ�.
	AtBool _RemoveObject( uint64 objectId );

	/// ���� ��� �������� ��ε� ĳ���� �Ѵ�.
	AtVoid _Broadcast( google::protobuf::Message& pkt, uint64 exceptId = 0 );

protected:
	unordered_map<uint64, ObjectPtr > m_objects;
};
