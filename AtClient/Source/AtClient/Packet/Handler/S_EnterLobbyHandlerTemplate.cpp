////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Client use handler. !!Auto Make File!!
////////////////////////////////////////////////////////////////////////////////////////////////////


//#include "pch.h"
#include "ServerPacketHandler.h"
#include "Room/S_EnterLobbyHandler.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
bool Handle_S_EnterLobbyTemplate( PacketSessionPtr& session, Protocol::S_EnterLobby& pkt )
{
	return S_EnterLobbyHandler::Handle( session, pkt );
}