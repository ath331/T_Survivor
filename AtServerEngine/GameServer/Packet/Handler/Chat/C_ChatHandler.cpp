////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_ChatHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_ChatHandler.h"
#include "Session/GameSession.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"
#include "Logic/Cheat/CheatManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_ChatHandler::Handle( PacketSessionPtr& session, Protocol::C_Chat& pkt )
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

	if ( CheatManager::GetInstance().IsCheat( pkt.msg() ) )
	{
		CheatManager::GetInstance().Run( player, pkt.msg() );
		return true;
	}

	AtInt64 senderId = player->GetId();

	room->ForeachPlayer(
		[ senderId, pkt ]( PlayerPtr eachPlayer )
		{
			if ( !eachPlayer )
				return;

			Protocol::S_Chat result;
			result.set_playerid( senderId  );
			result.set_msg     ( pkt.msg() );

			eachPlayer->Send( result );
		} );

	return true;
}
