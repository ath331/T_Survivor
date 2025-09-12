#include "pch.h"
#include "WaitingRoomManager.h"
#include "Logic/Room/WaitingRoom.h"
#include "Logic/Room/Lobby.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 업데이트 한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid WaitingRoomManager::Update( Millisecond curTime )
{
	if ( m_waitingRoomMap.empty() )
		return;

	// INFO_LOG( "WaitingRoomManager::Update()" );

	std::set< AtInt64 > deleteRoomSet;
	WaitingRoomMap copyRoomMap;

	{
		WRITE_LOCK;
		copyRoomMap = m_waitingRoomMap;
	}

	for ( auto& [ roomNum, room ] : copyRoomMap )
	{
		if ( !room )
		{
			deleteRoomSet.insert( roomNum );
			continue;
		}

		room->UpdateTick( curTime );

		RoomInfo roomInfo;
		room->ExportTo( roomInfo );

		if ( roomInfo.room_state() == ROOM_STATE_DESTROY_RESERVATION )
		{
			deleteRoomSet.insert( roomNum );
			continue;
		}
	}

	{
		WRITE_LOCK;
		for ( auto deleteRoomNum : deleteRoomSet )
		{
			auto iter = m_waitingRoomMap.find( deleteRoomNum );
			if ( iter == m_waitingRoomMap.end() )
				continue;

			WaitingRoomPtr waitingRoom = iter->second;
			if ( !waitingRoom )
				continue;

			RoomInfo roomInfo;
			waitingRoom->ExportTo( roomInfo );

			m_waitingRoomMap.erase( iter );

			S_DestroyRoom destroyRoomNotify;
			destroyRoomNotify.set_result( EResultCode::RESULT_CODE_SUCCESS );
			*destroyRoomNotify.mutable_roominfo() = roomInfo;

			GLobby->Broadcast( destroyRoomNotify );
		}
	}
}

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
WaitingRoomPtr WaitingRoomManager::AcquireRoom( const RoomInfo& roomInfo )
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
AtVoid WaitingRoomManager::ExportToAllRoomInfo( S_RequestAllRoomInfo& s_requestAllRoomInfo )
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
