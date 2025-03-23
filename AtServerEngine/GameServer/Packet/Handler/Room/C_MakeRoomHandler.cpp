////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_MakeRoomHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_MakeRoomHandler.h"
#include "Session/GameSession.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/WaitingRoomManager.h"
#include "Logic/Room/Lobby.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_MakeRoomHandler::Handle( PacketSessionPtr& session, Protocol::C_MakeRoom& pkt )
{
	auto gameSession = static_pointer_cast< GameSession >( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = gameSession->player.load();
	if ( !player )
		return false;

	RoomPtr oldRoom = player->room.load().lock();
	if ( !oldRoom )
		return false;

	if ( !dynamic_cast< Lobby* >( oldRoom.get() ) )
	{
		Protocol::S_MakeRoom result;
		result.set_result( Protocol::EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );
		player->Send( result );
		return false;
	}

	WaitingRoomPtr waitingRoom = WaitingRoomManager::GetInstance().AcquireRoom( pkt.roominfo() );

	waitingRoom->DoAsync(
		&Room::HandleEnterPlayer,
		player,
		(Room::CallbackFunc)( [ oldRoom, waitingRoom, player ]()
							  {
								  Protocol::S_MakeRoom result;
								  result.set_result( Protocol::EResultCode::RESULT_CODE_SUCCESS );
								  waitingRoom->ExportTo( result.mutable_maderoominfo() );

								  player->Send( result );

								  oldRoom->DoAsync(
									  &Room::HandleLeavePlayer,
									  player,
									  (Room::CallbackFunc)( []() {} ) );
							  } ) );

	return true;
}