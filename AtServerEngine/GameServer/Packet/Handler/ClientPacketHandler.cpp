#include "pch.h"
#include "ClientPacketHandler.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Room/Room.h"
#include "Session/GameSession.h"


PacketHandlerFunc GPacketHandler[ UINT16_MAX ];


bool Handle_INVALID( PacketSessionPtr& session, BYTE* buffer, int32 len )
{
	PacketHeader* header = reinterpret_cast<PacketHeader*>( buffer );
	// TODO : Log
	return false;
}
