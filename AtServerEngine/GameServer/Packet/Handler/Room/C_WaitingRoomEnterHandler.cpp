////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_WaitingRoomEnterHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_WaitingRoomEnterHandler.h"
#include "Logic/Room/WaitingRoom.h"
#include "Logic/Room/WaitingRoomManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_WaitingRoomEnterHandler::Handle( PacketSessionPtr& session, Protocol::C_WaitingRoomEnter& pkt )
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
		Protocol::S_WaitingRoomEnter result;
		result.set_result( Protocol::EResultCode::RESULT_CODE_NO_HAVE_ROOM );
		player->Send( result );
		return false;
	}

	waitingRoom->DoAsync(
		[ waitingRoom, player ]()
		{
			if ( !waitingRoom->CheckEnterRoom() )
			{
				Protocol::S_WaitingRoomEnter result;
				result.set_result( Protocol::EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );
				player->Send( result );
				return;
			}

			waitingRoom->DoAsync(
				&Room::HandleEnterPlayer,
				player,
				(Room::CallbackFunc)( [ player, waitingRoom ]()
									  {
										  Protocol::S_WaitingRoomEnter result;
										  result.set_result( Protocol::EResultCode::RESULT_CODE_SUCCESS );
										  waitingRoom->ExportTo( result.mutable_roominfo() );
										  player->Send( result );

										  Protocol::S_WaitingRoomEnterNotify notify;
										  player->objectInfo->CopyFrom( notify.player() );
										  waitingRoom->Broadcast( notify, player->GetId() );

										  waitingRoom->ForeachPlayer(
											  [ player ]( PlayerPtr eachPlayer )
											  {
												  Protocol::S_WaitingRoomEnterNotify notify;
												  eachPlayer->objectInfo->CopyFrom( notify.player() );
												  player->Send( notify );
											  },
											  player->GetId() );
									  } ) );
		} );

	return true;
}