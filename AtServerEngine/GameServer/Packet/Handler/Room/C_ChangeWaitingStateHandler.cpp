////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_ChangeWaitingStateHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_ChangeWaitingStateHandler.h"
#include "Session/GameSession.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"
#include "Logic/Room/WaitingRoom.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_ChangeWaitingStateHandler::Handle( PacketSessionPtr& session, C_ChangeWaitingState& pkt )
{
	auto gameSession = static_pointer_cast< GameSession >( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = gameSession->player.load();
	if ( !player )
		return false;

	RoomPtr room = player->room.load().lock();
	if ( !room )
		return false;

	WaitingRoomPtr waitingRoom = std::dynamic_pointer_cast< WaitingRoom >( room );
	if ( !waitingRoom )
	{
		S_ChangeWaitingState result;
		result.set_result( EResultCode::RESULT_CODE_NO_WAITING_ROOM );
		player->Send( result );
		return false;
	}

	EWaitingState waitingState = pkt.state();

	waitingRoom->DoAsync(
		[ waitingRoom, player, waitingState ]()
		{
			if ( waitingState == WAITING_STATE_RAEDY )
				waitingRoom->ReadyPlayer( player );
			else if( waitingState == WAITING_STATE_NONE )
				waitingRoom->ReadyCanclePlayer( player );

			S_ChangeWaitingState result;
			result.set_result( EResultCode::RESULT_CODE_SUCCESS );
			result.set_state( waitingState );
			player->Send( result );

			S_ChangeWaitingStateNotify notify;
			notify.mutable_player()->CopyFrom( *player->objectInfo );
			notify.set_state( waitingState );
			waitingRoom->Broadcast( notify, player->GetId() );
		} );

	return true;
}