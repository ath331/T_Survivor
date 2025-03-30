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
	/// 대기실 룸 맵 타입 정의
	using WaitingRoomMap = std::map< AtInt32, WaitingRoomPtr >;

private:
	/// Lock
	USE_LOCK;

private:
	/// 대기실 룸 맵 타입
	WaitingRoomMap m_waitingRoomMap;

public:
	/// 업데이트 한다.
	AtVoid Update();

	/// WaitingRoom을 반환한다.
	WaitingRoomPtr AcquireRoom( AtInt32 roomNum );

	/// WaitingRoom을 반환한다.
	WaitingRoomPtr AcquireRoom( const RoomInfo& roomInfo );

	/// WaitingRoom을 반환한다.
	WaitingRoomPtr GetRoom( AtInt32 roomNum );

	/// 대기실의 모든 룸의 정보를 내보낸다.
	AtVoid ExportToAllRoomInfo( S_RequestAllRoomInfo& s_requestAllRoomInfo );
};
