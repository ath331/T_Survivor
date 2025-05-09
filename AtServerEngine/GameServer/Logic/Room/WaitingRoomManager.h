#pragma once
////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoomManager File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "CoreMacro.h"
#include "Logic/Core/Singleton.h"
#include "Logic/Room/RoomTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoomManager class
////////////////////////////////////////////////////////////////////////////////////////////////////
class WaitingRoomManager :
	public Singleton< WaitingRoomManager >
{
private:
	/// ���� �� �� Ÿ�� ����
	using WaitingRoomMap = std::map<AtInt32, WaitingRoomPtr >;

private:
	/// Lock
	USE_LOCK;

private:
	/// ���� �� �� Ÿ��
	WaitingRoomMap m_waitingRoomMap;

public:
	/// WaitingRoom�� ��ȯ�Ѵ�.
	WaitingRoomPtr AcquireRoom( AtInt32 roomNum );
};
