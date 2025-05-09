////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif S_EnterGameHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AtClient.h"
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif S_EnterGameHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////
class S_EnterGameHandler
{
public:
	// HandlerRun
	static bool Handle( PacketSessionPtr& session, Protocol::S_EnterGame& pkt );
};
