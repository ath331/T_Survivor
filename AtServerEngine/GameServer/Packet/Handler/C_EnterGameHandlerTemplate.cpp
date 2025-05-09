////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Server use handler. !!Auto Make File!!
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "Logic/Utils/Log/AtLog.h"
#include "ClientPacketHandler.h"
#include "Game/C_EnterGameHandler.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
bool Handle_C_EnterGameTemplate( PacketSessionPtr& session, Protocol::C_EnterGame& pkt )
{
	PKT_LOG( pkt );

	return C_EnterGameHandler::Handle( session, pkt );
}