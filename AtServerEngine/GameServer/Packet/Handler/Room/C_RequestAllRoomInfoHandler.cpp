////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_RequestAllRoomInfoHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_RequestAllRoomInfoHandler.h"
#include "Logic/Room/Lobby.h"
#include "Logic/Room/WaitingRoomManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_RequestAllRoomInfoHandler::Handle( PacketSessionPtr& session, Protocol::C_RequestAllRoomInfo& pkt )
{
	auto gameSession = static_pointer_cast<GameSession>( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = gameSession->player.load();
	if ( !player )
		return false;

	RoomPtr curRoom = player->room.load().lock();
	if ( !curRoom )
		return false;

	if ( !dynamic_cast< Lobby* >( curRoom.get() ) )
	{
		Protocol::S_RequestAllRoomInfo result;
		result.set_result( Protocol::EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );
		player->Send( result );
		return false;
	}

	Protocol::S_RequestAllRoomInfo result;
	result.set_result( Protocol::EResultCode::RESULT_CODE_SUCCESS );

	WaitingRoomManager::GetInstance().ExportToAllRoomInfo( result );

	player->Send( result );

	return true;
}