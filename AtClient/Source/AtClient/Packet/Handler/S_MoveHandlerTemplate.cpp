////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Client use handler. !!Auto Make File!!
////////////////////////////////////////////////////////////////////////////////////////////////////


//#include "pch.h"
#include "ServerPacketHandler.h"
#include "Actor/S_MoveHandler.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
bool Handle_S_MoveTemplate( PacketSessionPtr& session, Protocol::S_Move& pkt )
{
	return S_MoveHandler::Handle( session, pkt );
}