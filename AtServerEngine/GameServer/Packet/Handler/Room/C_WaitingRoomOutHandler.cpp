////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_WaitingRoomOutHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_WaitingRoomOutHandler.h"
#include "Logic/Room/Lobby.h"
#include "Logic/Room/Room.h"
#include "Logic/Room/RoomTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_WaitingRoomOutHandler::Handle( PacketSessionPtr& session, C_WaitingRoomOut& pkt )
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

	//if ( !dynamic_cast< WaitingRoom* >( room.get() ) )
	//{
	//	S_WaitingRoomOut result;
	//	result.set_result( Protocol::EResultCode::RESULT_CODE_NO_WAITING_ROOM );
	//	player->Send( result );
	//}

	room->DoAsync(
		&Room::HandleLeavePlayer,
		player,
		(Room::CallbackFunc)( [ room, player ]()
							  {
								  GLobby->DoAsync(
									  [ player, room ]()
									  {
										  S_WaitingRoomOut result;
										  result.set_result( EResultCode::RESULT_CODE_SUCCESS );
										  player->Send( result );

										  S_RequestRoomInfo refreshRoomInfo;
										  refreshRoomInfo.set_result( EResultCode::RESULT_CODE_SUCCESS );
										  room->ExportTo( *refreshRoomInfo.mutable_roominfo() );

										  GLobby->Broadcast( refreshRoomInfo );
									  } );

								  S_WaitingRoomOutNotify notify;
								  notify.mutable_player()->CopyFrom( *player->objectInfo );
								  room->Broadcast( notify, player->GetId() );
							  } ) );

	return true;
}