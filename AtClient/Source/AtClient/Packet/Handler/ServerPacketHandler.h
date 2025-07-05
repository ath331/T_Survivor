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
bool Handle_ST_LoginTemplate(PacketSessionPtr& session, Protocol::ST_Login& pkt);
bool Handle_ST_EnterLobbyTemplate(PacketSessionPtr& session, Protocol::ST_EnterLobby& pkt);
bool Handle_ST_ServerListReadTemplate(PacketSessionPtr& session, Protocol::ST_ServerListRead& pkt);

class ServerPacketHandler
{
public:
	static void Init()
	{
		for (int32 i = 0; i < UINT16_MAX; i++)
			GPacketHandler[i] = Handle_INVALID;
		GPacketHandler[ (uint16)( EPacketId::PKT_ST_Login ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::ST_Login>(Handle_ST_LoginTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_ST_EnterLobby ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::ST_EnterLobby>(Handle_ST_EnterLobbyTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_ST_ServerListRead ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::ST_ServerListRead>(Handle_ST_ServerListReadTemplate, session, buffer, len); };
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
	else if ( packetTypeName == "Protocol.CT_Login" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_CT_Login ) );
	else if ( packetTypeName == "Protocol.CT_EnterLobby" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_CT_EnterLobby ) );
	else if ( packetTypeName == "Protocol.CT_ServerListRead" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_CT_ServerListRead ) );

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