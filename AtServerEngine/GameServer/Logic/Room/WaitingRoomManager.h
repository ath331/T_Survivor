#pragma once
////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoomManager File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "CoreMacro.h"
#include "Logic/Core/Singleton.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Room/WaitingRoom.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoomManager class
////////////////////////////////////////////////////////////////////////////////////////////////////
class WaitingRoomManager :
	public Singleton< WaitingRoomManager >
{
private:
	/// ���� �� �� Ÿ�� ����
	using WaitingRoomMap = std::map< AtInt32, WaitingRoomPtr >;

private:
	/// Lock
	USE_LOCK;

private:
	/// ���� �� �� Ÿ��
	WaitingRoomMap m_waitingRoomMap;

public:
	/// ������Ʈ �Ѵ�.
	AtVoid Update();

	/// WaitingRoom�� ��ȯ�Ѵ�.
	WaitingRoomPtr AcquireRoom( AtInt32 roomNum );

	/// WaitingRoom�� ��ȯ�Ѵ�.
	WaitingRoomPtr AcquireRoom( const RoomInfo& roomInfo );

	/// WaitingRoom�� ��ȯ�Ѵ�.
	WaitingRoomPtr GetRoom( AtInt32 roomNum );

	/// ������ ��� ���� ������ ��������.
	AtVoid ExportToAllRoomInfo( S_RequestAllRoomInfo& s_requestAllRoomInfo );
};
