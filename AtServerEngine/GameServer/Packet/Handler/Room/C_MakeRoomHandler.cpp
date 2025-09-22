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
		S_MakeRoom result;
		result.set_result( EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );
		player->Send( result );
		return false;
	}

	WaitingRoomPtr newRoom = WaitingRoomManager::GetInstance().AcquireRoom( pkt.roominfo() );

	oldRoom->DoAsync(
		[ oldRoom, newRoom, player ]()
		{
			oldRoom->HandleLeavePlayer( player );

			newRoom->HandleEnterPlayer(
				player,
				[ newRoom, player ]()
				{
					S_MakeRoom result;
					result.set_result( EResultCode::RESULT_CODE_SUCCESS );
					newRoom->ExportTo( *result.mutable_maderoominfo() );
					result.mutable_player()->CopyFrom( *player->objectInfo );

					player->Send( result );

					GLobby->DoAsync(
						[ newRoom ]()
						{
							S_RequestRoomInfo refreshRoomInfo;
							refreshRoomInfo.set_result( EResultCode::RESULT_CODE_SUCCESS );
							newRoom->ExportTo( *refreshRoomInfo.mutable_roominfo() );

							GLobby->Broadcast( refreshRoomInfo );
						} );
				} );
		} );

	return true;
}