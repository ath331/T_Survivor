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
	AtBool HandleEnterPlayer( PlayerPtr player );

	/// �÷��̾ �濡�� ��������.
	AtBool HandleLeavePlayer( PlayerPtr player );

	/// �÷��̾��� �������� ó���Ѵ�.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

public:
	/// ���� ������Ʈ�Ѵ�.
	AtVoid UpdateTick();

	/// Room��ü�� ��ȯ�Ѵ�.
	RoomPtr GetPtr();

private:
	/// ������Ʈ�� �߰��Ѵ�.
	AtBool AddObject( ObjectPtr object );

	/// ������Ʈ�� �����Ѵ�.
	AtBool RemoveObject( uint64 objectId );

private:
	AtVoid Broadcast( SendBufferPtr sendBuffer, uint64 exceptId = 0 );

private:
	unordered_map<uint64, ObjectPtr > m_objects;
};

extern RoomPtr GRoom;