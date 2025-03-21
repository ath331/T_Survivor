////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_LeaveGameHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_LeaveGameHandler.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Session/GameSession.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_LeaveGameHandler::Handle( PacketSessionPtr& session, Protocol::C_LeaveGame& pkt )
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

	room->DoAsync( 
		&Room::HandleLeavePlayer, 
		player, 
		(Room::CallbackFunc)( [ player, room ]()
							  {
								  // 퇴장 사실을 퇴장하는 플레이어에게 알린다
								  {
									  Protocol::S_LeaveGame leaveGamePkt;
									  player->Send( leaveGamePkt );
								  }

								  // 퇴장 사실을 알린다
								  {
									  Protocol::S_DeSpawn despawnPkt;
									  despawnPkt.add_ids( player->GetId() );

									  room->Broadcast( despawnPkt, player->GetId() );
									  player->Send( despawnPkt );
								  }
							  } ) );

	return true;
}