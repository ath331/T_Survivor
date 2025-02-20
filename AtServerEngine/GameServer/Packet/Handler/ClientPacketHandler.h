#pragma once
#include "PacketId.h"
#include "Packet/Protocol.pb.h"


#if UE_BUILD_DEBUG + UE_BUILD_DEVELOPMENT + UE_BUILD_TEST + UE_BUILD_SHIPPING >= 1
#include "AtClient.h"
#endif


using PacketHandlerFunc = std::function<bool(PacketSessionPtr&, BYTE*, int32)>;
extern PacketHandlerFunc GPacketHandler[UINT16_MAX];


// Custom Handlers
bool Handle_INVALID(PacketSessionPtr& session, BYTE* buffer, int32 len);
bool Handle_C_LoginTemplate(PacketSessionPtr& session, Protocol::C_Login& pkt);
bool Handle_C_EnterLobbyTemplate(PacketSessionPtr& session, Protocol::C_EnterLobby& pkt);
bool Handle_C_EnterGameTemplate(PacketSessionPtr& session, Protocol::C_EnterGame& pkt);
bool Handle_C_LeaveGameTemplate(PacketSessionPtr& session, Protocol::C_LeaveGame& pkt);
bool Handle_C_MoveTemplate(PacketSessionPtr& session, Protocol::C_Move& pkt);
bool Handle_C_ChatTemplate(PacketSessionPtr& session, Protocol::C_Chat& pkt);
bool Handle_C_WaitingRoomEnterTemplate(PacketSessionPtr& session, Protocol::C_WaitingRoomEnter& pkt);

class ClientPacketHandler
{
public:
	static void Init()
	{
		for (int32 i = 0; i < UINT16_MAX; i++)
			GPacketHandler[i] = Handle_INVALID;
		GPacketHandler[ (uint16)( EPacketId::PKT_C_Login ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_Login>(Handle_C_LoginTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_EnterLobby ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_EnterLobby>(Handle_C_EnterLobbyTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_EnterGame ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_EnterGame>(Handle_C_EnterGameTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_LeaveGame ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_LeaveGame>(Handle_C_LeaveGameTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_Move ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_Move>(Handle_C_MoveTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_Chat ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_Chat>(Handle_C_ChatTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_C_WaitingRoomEnter ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::C_WaitingRoomEnter>(Handle_C_WaitingRoomEnterTemplate, session, buffer, len); };
	}

	static bool HandlePacket(PacketSessionPtr& session, BYTE* buffer, int32 len)
	{
		PacketHeader* header = reinterpret_cast<PacketHeader*>(buffer);
		return GPacketHandler[header->id](session, buffer, len);
	}

static SendBufferPtr MakeSendBuffer( google::protobuf::Message& pkt )
{
	string packetTypeName = pkt.GetTypeName();
	if ( packetTypeName.empty() )
		return nullptr;
	else if ( packetTypeName == "Protocol.S_Login" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Login ) );
	else if ( packetTypeName == "Protocol.S_EnterLobby" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_EnterLobby ) );
	else if ( packetTypeName == "Protocol.S_EnterGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_EnterGame ) );
	else if ( packetTypeName == "Protocol.S_LeaveGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_LeaveGame ) );
	else if ( packetTypeName == "Protocol.S_Move" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Move ) );
	else if ( packetTypeName == "Protocol.S_Spawn" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Spawn ) );
	else if ( packetTypeName == "Protocol.S_DeSpawn" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_DeSpawn ) );
	else if ( packetTypeName == "Protocol.S_Chat" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Chat ) );
	else if ( packetTypeName == "Protocol.S_WaitingRoomEnter" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_WaitingRoomEnter ) );

	return nullptr;
}

private:
	template<typename PacketType, typename ProcessFunc>
	static bool HandlePacket(ProcessFunc func, PacketSessionPtr& session, BYTE* buffer, int32 len)
	{
		PacketType pkt;
		if (pkt.ParseFromArray(buffer + sizeof(PacketHeader), len - sizeof(PacketHeader)) == false)
			return false;

		return func(session, pkt);
	}

	template<typename T>
	static SendBufferPtr MakeSendBuffer(T& pkt, uint16 pktId)
	{
		const uint16 dataSize = static_cast<uint16>(pkt.ByteSizeLong());
		const uint16 packetSize = dataSize + sizeof(PacketHeader);

		//SendBufferPtr sendBuffer = GSendBufferManager->Open( packetSize );

	#if UE_BUILD_DEBUG + UE_BUILD_DEVELOPMENT + UE_BUILD_TEST + UE_BUILD_SHIPPING >= 1
		SendBufferPtr sendBuffer = MakeShared< SendBuffer >( packetSize );
	#else
		SendBufferPtr sendBuffer = make_shared< SendBuffer >( packetSize );
	#endif

		PacketHeader* header = reinterpret_cast<PacketHeader*>(sendBuffer->Buffer());
		header->size = packetSize;
		header->id = pktId;
		//ASSERT_CRASH( pkt.SerializeToArray( &header[ 1 ], dataSize ) );
		pkt.SerializeToArray( &header[ 1 ], dataSize );
		sendBuffer->Close(packetSize);

		return sendBuffer;
	}
};