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
bool Handle_S_LoginTemplate(PacketSessionPtr& session, Protocol::S_Login& pkt);
bool Handle_S_EnterLobbyTemplate(PacketSessionPtr& session, Protocol::S_EnterLobby& pkt);
bool Handle_S_EnterGameTemplate(PacketSessionPtr& session, Protocol::S_EnterGame& pkt);
bool Handle_S_EnterGameFinishTemplate(PacketSessionPtr& session, Protocol::S_EnterGameFinish& pkt);
bool Handle_S_LeaveGameTemplate(PacketSessionPtr& session, Protocol::S_LeaveGame& pkt);
bool Handle_S_MoveTemplate(PacketSessionPtr& session, Protocol::S_Move& pkt);
bool Handle_S_SpawnTemplate(PacketSessionPtr& session, Protocol::S_Spawn& pkt);
bool Handle_S_DeSpawnTemplate(PacketSessionPtr& session, Protocol::S_DeSpawn& pkt);
bool Handle_S_ChatTemplate(PacketSessionPtr& session, Protocol::S_Chat& pkt);
bool Handle_S_WaitingRoomEnterTemplate(PacketSessionPtr& session, Protocol::S_WaitingRoomEnter& pkt);
bool Handle_S_AnimationEventTemplate(PacketSessionPtr& session, Protocol::S_AnimationEvent& pkt);

class ServerPacketHandler
{
public:
	static void Init()
	{
		for (int32 i = 0; i < UINT16_MAX; i++)
			GPacketHandler[i] = Handle_INVALID;
		GPacketHandler[ (uint16)( EPacketId::PKT_S_Login ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_Login>(Handle_S_LoginTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_EnterLobby ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_EnterLobby>(Handle_S_EnterLobbyTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_EnterGame ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_EnterGame>(Handle_S_EnterGameTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_EnterGameFinish ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_EnterGameFinish>(Handle_S_EnterGameFinishTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_LeaveGame ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_LeaveGame>(Handle_S_LeaveGameTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_Move ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_Move>(Handle_S_MoveTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_Spawn ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_Spawn>(Handle_S_SpawnTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_DeSpawn ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_DeSpawn>(Handle_S_DeSpawnTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_Chat ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_Chat>(Handle_S_ChatTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_WaitingRoomEnter ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_WaitingRoomEnter>(Handle_S_WaitingRoomEnterTemplate, session, buffer, len); };
		GPacketHandler[ (uint16)( EPacketId::PKT_S_AnimationEvent ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::S_AnimationEvent>(Handle_S_AnimationEventTemplate, session, buffer, len); };
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
	else if ( packetTypeName == "Protocol.C_Login" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_Login ) );
	else if ( packetTypeName == "Protocol.C_EnterLobby" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_EnterLobby ) );
	else if ( packetTypeName == "Protocol.C_EnterGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_EnterGame ) );
	else if ( packetTypeName == "Protocol.C_EnterGameFinish" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_EnterGameFinish ) );
	else if ( packetTypeName == "Protocol.C_LeaveGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_LeaveGame ) );
	else if ( packetTypeName == "Protocol.C_Move" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_Move ) );
	else if ( packetTypeName == "Protocol.C_Chat" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_Chat ) );
	else if ( packetTypeName == "Protocol.C_WaitingRoomEnter" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_WaitingRoomEnter ) );
	else if ( packetTypeName == "Protocol.C_AnimationEvent" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_C_AnimationEvent ) );

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