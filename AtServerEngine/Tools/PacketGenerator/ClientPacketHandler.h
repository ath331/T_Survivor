#pragma once
#include "../PacketId.h"
#include "Packet/Protocol.pb.h"


#if UE_BUILD_DEBUG + UE_BUILD_DEVELOPMENT + UE_BUILD_TEST + UE_BUILD_SHIPPING >= 1
#include "AtClient.h"
#endif


using PacketHandlerFunc = std::function<bool(PacketSessionPtr&, BYTE*, int32)>;
extern PacketHandlerFunc GPacketHandler[UINT16_MAX];


// Custom Handlers
bool Handle_INVALID(PacketSessionPtr& session, BYTE* buffer, int32 len);
bool Handle_CT_ctTestTemplate(PacketSessionPtr& session, Protocol::CT_ctTest& pkt);

class ClientPacketHandler
{
public:
	static void Init()
	{
		for (int32 i = 0; i < UINT16_MAX; i++)
			GPacketHandler[i] = Handle_INVALID;
		GPacketHandler[ (uint16)( EPacketId::PKT_CT_ctTest ) ] = [](PacketSessionPtr& session, BYTE* buffer, int32 len) { return HandlePacket<Protocol::CT_ctTest>(Handle_CT_ctTestTemplate, session, buffer, len); };
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
	else if ( packetTypeName == "Protocol.S_WaitingRoomEnter" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_WaitingRoomEnter ) );
	else if ( packetTypeName == "Protocol.S_WaitingRoomEnterNotify" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_WaitingRoomEnterNotify ) );
	else if ( packetTypeName == "Protocol.S_MakeRoom" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_MakeRoom ) );
	else if ( packetTypeName == "Protocol.S_DestroyRoom" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_DestroyRoom ) );
	else if ( packetTypeName == "Protocol.S_RequestRoomInfo" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_RequestRoomInfo ) );
	else if ( packetTypeName == "Protocol.S_RequestAllRoomInfo" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_RequestAllRoomInfo ) );
	else if ( packetTypeName == "Protocol.S_WaitingRoomOut" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_WaitingRoomOut ) );
	else if ( packetTypeName == "Protocol.S_WaitingRoomOutNotify" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_WaitingRoomOutNotify ) );
	else if ( packetTypeName == "Protocol.S_ChangeWaitingState" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_ChangeWaitingState ) );
	else if ( packetTypeName == "Protocol.S_ChangeWaitingStateNotify" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_ChangeWaitingStateNotify ) );
	else if ( packetTypeName == "Protocol.S_ChangeRoomLeaderNotify" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_ChangeRoomLeaderNotify ) );
	else if ( packetTypeName == "Protocol.S_EnterGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_EnterGame ) );
	else if ( packetTypeName == "Protocol.S_EnterGameFinish" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_EnterGameFinish ) );
	else if ( packetTypeName == "Protocol.S_LeaveGame" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_LeaveGame ) );
	else if ( packetTypeName == "Protocol.S_Move" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Move ) );
	else if ( packetTypeName == "Protocol.S_Spawn" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Spawn ) );
	else if ( packetTypeName == "Protocol.S_DeSpawn" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_DeSpawn ) );
	else if ( packetTypeName == "Protocol.S_Chat" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_Chat ) );
	else if ( packetTypeName == "Protocol.S_AnimationEvent" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_AnimationEvent ) );
	else if ( packetTypeName == "Protocol.S_ServerListRead" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_S_ServerListRead ) );
	else if ( packetTypeName == "Protocol.ST_stTest" ) return MakeSendBuffer( pkt, (uint16)( EPacketId::PKT_ST_stTest ) );

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