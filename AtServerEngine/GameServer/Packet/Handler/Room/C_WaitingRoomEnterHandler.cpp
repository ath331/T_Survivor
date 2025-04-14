////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_WaitingRoomEnterHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_WaitingRoomEnterHandler.h"
#include "Logic/Room/Lobby.h"
#include "Logic/Room/WaitingRoom.h"
#include "Logic/Room/WaitingRoomManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_WaitingRoomEnterHandler::Handle( PacketSessionPtr& session, C_WaitingRoomEnter& pkt )
{
	auto gameSession = static_pointer_cast<GameSession>( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = gameSession->player.load();
	if ( !player )
		return false;

	AtInt32 roomNum = pkt.roominfo().num();

	WaitingRoomPtr waitingRoom = WaitingRoomManager::GetInstance().GetRoom( roomNum );
	if ( !waitingRoom )
	{
		S_WaitingRoomEnter result;
		result.set_result( EResultCode::RESULT_CODE_NO_HAVE_ROOM );
		player->Send( result );
		return false;
	}

	RoomPtr oldRoom = player->room.load().lock();
	if ( !oldRoom )
	{
		S_WaitingRoomEnter result;
		result.set_result( EResultCode::RESULT_CODE_NO_HAVE_ROOM );
		player->Send( result );
		return false;
	}

	if ( !dynamic_cast<Lobby*>( oldRoom.get() ) )
	{
		S_WaitingRoomEnter result;
		result.set_result( EResultCode::RESULT_CODE_NO_LOBBY_ROOM );
		player->Send( result );
		return false;
	}

	oldRoom->DoAsync(
		[ oldRoom, waitingRoom, player ]()
		{
			oldRoom->HandleLeavePlayer(
				player,
				[ waitingRoom, player ]()
				{
					waitingRoom->DoAsync(
						[ waitingRoom, player ]()
						{
							if ( !waitingRoom->CheckEnterRoom() )
							{
								S_WaitingRoomEnter result;
								result.set_result( EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );
								player->Send( result );
								return;
							}

							waitingRoom->HandleEnterPlayer(
								player,
								[ waitingRoom, player ]()
								{
									S_WaitingRoomEnter result;
									result.set_result( EResultCode::RESULT_CODE_SUCCESS );
									waitingRoom->ExportTo( *result.mutable_roominfo() );
									player->Send( result );

									S_WaitingRoomEnterNotify notify;
									notify.mutable_player()->CopyFrom( *player->objectInfo );
									waitingRoom->Broadcast( notify, player->GetId() );

									waitingRoom->ForeachPlayer(
										[ player ]( PlayerPtr eachPlayer )
										{
											S_WaitingRoomEnterNotify notify;
											notify.mutable_player()->CopyFrom( *eachPlayer->objectInfo );
											player->Send( notify );
										},
										player->GetId() );

									GLobby->DoAsync(
										[ waitingRoom ]()
										{
											S_RequestRoomInfo refreshRoomInfo;
											refreshRoomInfo.set_result( EResultCode::RESULT_CODE_SUCCESS );
											waitingRoom->ExportTo( *refreshRoomInfo.mutable_roominfo() );

											GLobby->Broadcast( refreshRoomInfo );
										} );
								} );

						} );

				} );
		} );

	return true;
}