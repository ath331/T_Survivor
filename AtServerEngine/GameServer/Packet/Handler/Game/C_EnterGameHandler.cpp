////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_EnterGameHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_EnterGameHandler.h"
#include "Logic/Utils/Utils.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Utils/Time/AtTime.h"
#include "Logic/Utils/ObjectUtils.h"
#include "Logic/Room/PlayRoom.h"
#include "Logic/Room/PlayRoomManager.h"
#include "Logic/Room/WaitingRoom.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Session/GameSession.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_EnterGameHandler::Handle( PacketSessionPtr& session, C_EnterGame& pkt )
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

	WaitingRoomPtr waitingRoom = std::dynamic_pointer_cast<WaitingRoom>( room );
	if ( !waitingRoom )
	{
		S_EnterGame enterGamePkt;
		enterGamePkt.set_result( EResultCode::RESULT_CODE_NO_WAITING_ROOM );
		player->Send( enterGamePkt );
		return false;
	}

	PlayRoomPtr playRoom = PlayRoomManager::GetInstance().CreateRoom();
	if ( !playRoom )
		return false;

	playRoom->UpdateTick( AtTime::GetCurMillisecond() );

	waitingRoom->DoAsync(
		[ waitingRoom, playRoom, player ]()
		{
			if ( !waitingRoom->IsAllPlayerReady() )
			{
				S_EnterGame enterGamePkt;
				enterGamePkt.set_result( EResultCode::RESULT_CODE_NO_RAEDY_ALL_MEMBER );
				player->Send( enterGamePkt );
				return;
			}

			waitingRoom->ForeachPlayer(
				[ playRoom ]( PlayerPtr eachPlayer )
				{
					eachPlayer->posInfo->set_x( Utils::GetRandom( -15.f, 15.f ) );
					eachPlayer->posInfo->set_z( Utils::GetRandom( -15.f, 15.f ) );
					eachPlayer->posInfo->set_y( 0.0f );
					//eachPlayer->posInfo->set_yaw( Utils::GetRandom( 0.f, 100.f ) );

					playRoom->DoAsync(
						&Room::HandleEnterPlayer,
						eachPlayer,
						(Room::CallbackFunc)( [ eachPlayer ]()
											  {
												  Protocol::S_EnterGame enterGamePkt;
												  enterGamePkt.set_result( Protocol::EResultCode::RESULT_CODE_SUCCESS );

												  Protocol::ObjectInfo* playerInfo = new Protocol::ObjectInfo();
												  playerInfo->CopyFrom( *eachPlayer->objectInfo );
												  enterGamePkt.set_allocated_player( playerInfo );

												  if ( auto session = eachPlayer->session.lock() )
													  session->Send( enterGamePkt );
											  } ) );
				} );
		} );


	return true;
}