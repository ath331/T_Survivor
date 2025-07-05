////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_EnterLobbyHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_EnterLobbyHandler.h"
#include "Session/GameSession.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Room/Lobby.h"
#include "Logic/Room/WaitingRoomManager.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Utils/ObjectUtils.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_EnterLobbyHandler::Handle( PacketSessionPtr& session, C_EnterLobby& pkt )
{
	auto gameSession = static_pointer_cast<GameSession>( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = ObjectUtils::CreatePlayer( gameSession );
	player->objectInfo->set_id( gameSession->GetSessionId() );

	GLobby->DoAsync(
		&Room::HandleEnterPlayer,
		player,
		(Room::CallbackFunc)( [ player ]()
							  {
								  S_EnterLobby result;
								  result.set_success( true );
								  result.set_playerid( player->GetId() );

								  player->Send( result );

								  // S_RequestAllRoomInfo allRoomInfo;
								  // WaitingRoomManager::GetInstance().ExportToAllRoomInfo( allRoomInfo );
								  // player->Send( allRoomInfo );

							  } ) );

	return true;
}