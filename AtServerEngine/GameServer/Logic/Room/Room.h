////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room class
////////////////////////////////////////////////////////////////////////////////////////////////////
class Room 
	: 
	public JobQueue
{
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
	AtVoid _Broadcast( SendBufferPtr sendBuffer, uint64 exceptId = 0 );

protected:
	unordered_map<uint64, ObjectPtr > m_objects;
};

extern RoomPtr GRoom;