#include "pch.h"
#include "GameSession.h"
#include "GameSessionManager.h"
#include "Packet/Handler/PacketId.h"
#include "Packet/Handler/ClientPacketHandler.h"
#include "Utils/Log/AtLog.h"
#include "Utils/String/StringUtils.h"


static std::atomic< AtInt64 > GSessionId = 0;


void GameSession::OnConnected()
{
	GSessionId++;

	SetSessionId( GSessionId );
	GSessionManager.Add( static_pointer_cast< GameSession >( shared_from_this() ) );

	INFO_LOG( StringUtils::ConvertToString( GetAddress().GetIpAddress() ) + " Connected." );
}

void GameSession::OnDisconnected()
{
	GSessionManager.Remove( static_pointer_cast< GameSession >( shared_from_this() ) );

	INFO_LOG( StringUtils::ConvertToString( GetAddress().GetIpAddress() ) + " Disconnected." );
}

void GameSession::OnRecvPacket( BYTE* buffer, int32 len )
{
	PacketSessionPtr session = GetPacketSessionRef();
	PacketHeader* header = reinterpret_cast< PacketHeader* >( buffer );
	if ( !header )
	{
		WARNNING_LOG( "PacketHeader is null" );
		return;
	}

	// TODO : packetId 대역 체크
	ClientPacketHandler::HandlePacket( session, buffer, len );
}

void GameSession::OnSend(int32 len)
{
	// PKT_LOG( "[Send]" );
}

void GameSession::Send( SendBufferPtr sendBuffer )
{
	if ( !sendBuffer )
	{
		WARNNING_LOG( "SendBuffer is null" );
		return;
	}

	PacketHeader* header = reinterpret_cast<PacketHeader*>( sendBuffer->Buffer() );
	if ( !header )
	{
		WARNNING_LOG( "PacketHeader is null" );
		return;
	}

	INFO_LOG_NO_LINE( "[Send] " + std::to_string( header->id ) );

	PacketSession::Send( sendBuffer );
}
