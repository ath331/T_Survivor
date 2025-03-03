////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_AnimationEventHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_AnimationEventHandler.h"
#include "Session/GameSession.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_AnimationEventHandler::Handle( PacketSessionPtr& session, Protocol::C_AnimationEvent& pkt )
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

	room->ForeachPlayer(
		[ pkt ]( PlayerPtr eachPlayer )
		{
			if ( !eachPlayer )
				return;

			Protocol::S_AnimationEvent result;
			result.set_result       ( Protocol::EResultCode::RESULT_CODE_SUCCESS );
			result.set_playerid     ( eachPlayer->GetId() );
			result.set_animationtype( pkt.animationtype() );
			result.set_paramtype    ( pkt.paramtype()     );
			result.set_boolvalue    ( pkt.boolvalue()     );
			result.set_floatvalue   ( pkt.floatvalue()    );

			eachPlayer->Send( result );
		} );

	return true;
}