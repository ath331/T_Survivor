////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif C_EnterLobbyHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "C_EnterLobbyHandler.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Room/Lobby.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Utils/ObjectUtils.h"
#include "Session/GameSession.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool C_EnterLobbyHandler::Handle( PacketSessionPtr& session, Protocol::C_EnterLobby& pkt )
{
	auto gameSession = static_pointer_cast<GameSession>( session );
	if ( !gameSession )
		return false;

	PlayerPtr player = ObjectUtils::CreatePlayer( gameSession );
	player->objectInfo->set_id( gameSession->GetSessionId() );

	GLobby->DoAsync( &Room::HandleEnterPlayer, player );

	return true;
}