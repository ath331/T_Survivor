#include "pch.h"
#include "PlayRoomManager.h"
#include "Logic/Room/PlayRoom.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 룸을 생성한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
PlayRoomPtr PlayRoomManager::CreateRoom( const AtString& mapName )
{
	WRITE_LOCK;
	auto playRoom = std::make_shared< PlayRoom >( mapName );

	m_playRoomMap[ playRoom->GetRoomNum() ] = playRoom;
	return playRoom;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 룸을 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
PlayRoomPtr PlayRoomManager::AcquireRoom( AtInt32 roomNum, const AtString& mapName )
{
	WRITE_LOCK;

	auto iter = m_playRoomMap.find( roomNum );
	if ( iter != m_playRoomMap.end() )
		return iter->second;

	auto playRoom = std::make_shared< PlayRoom >( mapName );
	return playRoom;
}
