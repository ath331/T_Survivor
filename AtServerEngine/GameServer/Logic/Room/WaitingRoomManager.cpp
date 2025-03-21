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
	m_waitingRoomMap[ waitingRoom->GetRoomNum() ] = waitingRoom;

	return waitingRoom;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoom을 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
WaitingRoomPtr WaitingRoomManager::AcquireRoom( const Protocol::RoomInfo& roomInfo )
{
	WRITE_LOCK;

	auto iter = m_waitingRoomMap.find( roomInfo.num() );
	if ( iter != m_waitingRoomMap.end() )
		return iter->second;

	auto waitingRoom = std::make_shared< WaitingRoom >( roomInfo.max_count(), roomInfo.name(), roomInfo.pw() );
	m_waitingRoomMap[ waitingRoom->GetRoomNum() ] = waitingRoom;

	return waitingRoom;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoom을 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
WaitingRoomPtr WaitingRoomManager::GetRoom( AtInt32 roomNum )
{
	WRITE_LOCK;

	auto iter = m_waitingRoomMap.find( roomNum );
	if ( iter != m_waitingRoomMap.end() )
		return iter->second;

	return nullptr;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 대기실의 모든 룸의 정보를 내보낸다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid WaitingRoomManager::ExportToAllRoomInfo( Protocol::S_RequestAllRoomInfo& s_requestAllRoomInfo )
{
	WRITE_LOCK;

	for ( const auto& [ roomNum, room ] : m_waitingRoomMap )
	{
		if ( !room )
			continue;

		auto descRoomInfo = s_requestAllRoomInfo.add_roomlist();
		room->ExportTo( *descRoomInfo );
	}
}
