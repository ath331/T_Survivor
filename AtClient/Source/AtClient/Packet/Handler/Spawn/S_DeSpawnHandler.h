////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif S_DeSpawnHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AtClient.h"
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif S_DeSpawnHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////
class S_DeSpawnHandler
{
public:
	// HandlerRun
	static bool Handle( PacketSessionPtr& session, Protocol::S_DeSpawn& pkt );
};
