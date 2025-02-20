#include "pch.h"
#include "WaitingRoomManager.h"
#include "Logic/Room/WaitingRoom.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoom을 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
WaitingRoomPtr WaitingRoomManager::AcquireRoom( AtInt32 roomNum )
{
	WRITE_LOCK;

	auto iter = m_waitingRoomMap.find( roomNum );
	if ( iter != m_waitingRoomMap.end() )
		return iter->second;

	auto waitingRoom = std::make_shared< WaitingRoom >();
	return waitingRoom;
}
