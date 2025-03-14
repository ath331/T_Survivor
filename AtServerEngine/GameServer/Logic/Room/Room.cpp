////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "Room.h"
#include "CoreMacro.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Lock.h"
#include "Session/GameSession.h"
#include "Packet/Handler/ClientPacketHandler.h"


std::atomic< AtInt32 > Room::roomNum( 0 );


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::Room()
{
	roomNum.fetch_add( 1 );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소멸자
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::~Room()
{

}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어를 방에 입장시킨다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleEnterPlayer( PlayerPtr player, CallbackFunc callback )
{
	AtBool success = _AddObject( player );

	if ( !success )
		return false;

	if ( callback )
		callback();

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어를 방에서 내보낸다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleLeavePlayer( PlayerPtr player )
{
	if ( player == nullptr )
		return false;
	
	const uint64 objectId = player->objectInfo->id();
	bool success = _RemoveObject( objectId );
	
	// 퇴장 사실을 퇴장하는 플레이어에게 알린다
	{
		Protocol::S_LeaveGame leaveGamePkt;
	
		if ( auto session = player->session.lock() )
			session->Send( leaveGamePkt );
	}
	
	// 퇴장 사실을 알린다
	{
		Protocol::S_DeSpawn despawnPkt;
		despawnPkt.add_ids( objectId );
	
		Broadcast( despawnPkt, objectId );
	
		if ( auto session = player->session.lock() )
			session->Send( despawnPkt );
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어의 움직임을 처리한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::HandlePlayerMove( Protocol::C_Move pkt )
{
	const AtInt64 id = pkt.objectinfo().id();
	if ( m_objects.find( id ) == m_objects.end() )
		return;

	// TODO : 위치 체크하기
	PlayerPtr player = dynamic_pointer_cast<Player>( m_objects[ id ] );
	if ( !player )
		return;

	player->posInfo->CopyFrom( pkt.objectinfo().pos_info() );

	Protocol::S_Move movePkt;
	auto* info =  movePkt.mutable_objectinfo();
	info->CopyFrom( pkt.objectinfo() );

	Broadcast( movePkt, id );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어들을 순회한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::ForeachPlayer( CallbackPlayer callback, AtInt64 exceptId )
{
	READ_LOCK;

	for ( const auto& [ playerId, player ] : m_players )
	{
		if ( !player )
			continue;

		callback( player );
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어끼리 싱크한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::SyncPlayers( PlayerPtr enterPlayer )
{
	if ( !enterPlayer )
		return;

	// 입장 사실을 다른 플레이어에게 알린다
	{
		Protocol::S_Spawn spawnPkt;

		Protocol::ObjectInfo* playerInfo = spawnPkt.add_objectlist();
		playerInfo->CopyFrom( *enterPlayer->objectInfo );

		Broadcast( spawnPkt, enterPlayer->objectInfo->id() );
	}

	// 기존 입장한 플레이어 목록을 신입 플레이어한테 전송해준다
	{
		Protocol::S_Spawn spawnPkt;

		for ( auto& item : m_objects )
		{
			if ( !item.second->IsPlayer() )
				continue;

			Protocol::ObjectInfo* playerInfo = spawnPkt.add_objectlist();
			playerInfo->CopyFrom( *item.second->objectInfo );
		}

		if ( auto session = enterPlayer->session.lock() )
			session->Send( spawnPkt );
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room객체를 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
RoomPtr Room::GetPtr()
{
	return static_pointer_cast<Room>( shared_from_this() );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room객체를 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::UpdateTick()
{
	DoAsync( &Room::UpdateTick );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 룸의 모든 유저에게 브로드 캐스팅 한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::Broadcast( google::protobuf::Message& pkt, uint64 exceptId )
{
	SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( pkt );

	ForeachPlayer(
		[ sendBuffer, exceptId ]( PlayerPtr eachPlayer )
		{
			if ( !eachPlayer )
				return;

			if ( eachPlayer->GetId() == exceptId )
				return;

			if ( GameSessionPtr session = eachPlayer->session.lock() )
				session->Send( sendBuffer );
		} );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 오브젝트를 추가한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::_AddObject( ObjectPtr object )
{
	WRITE_LOCK;

	if ( m_objects.find( object->objectInfo->id() ) != m_objects.end() )
		return false;

	m_objects.insert( make_pair( object->objectInfo->id(), object ) );

	object->room.store( GetPtr() );

	if ( PlayerPtr player = std::dynamic_pointer_cast<Player>( object ) )
	{
		m_players[ player->GetId() ] = player;
	}

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 오브젝트를 제거한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::_RemoveObject( uint64 objectId )
{
	WRITE_LOCK;

	if ( m_objects.find( objectId ) == m_objects.end() )
		return false;

	ObjectPtr object = m_objects[ objectId ];
	if ( !object )
		return false;

	object->room.store( weak_ptr<Room>() );

	m_objects.erase( objectId );

	auto iter = m_players.find( objectId );
	m_players.erase( iter );

	return true;
}