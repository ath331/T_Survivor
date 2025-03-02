////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_EnterGameFinishHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_EnterGameFinishHandler.h"
#include "Session/GameSession.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_EnterGameFinishHandler::Handle( PacketSessionPtr& session, Protocol::C_EnterGameFinish& pkt )
{
	auto gameSession = static_pointer_cast<GameSession>( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = gameSession->player.load();
	if ( !player )
		return false;

	RoomPtr room = player->room.load().lock();
	if ( !room )
		return false;

	room->SyncPlayers( player );

	return true;
}