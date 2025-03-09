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
// @breif ������
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::Room()
{
	roomNum.fetch_add( 1 );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �Ҹ���
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::~Room()
{

}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �÷��̾ �濡 �����Ų��.
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
// @breif �÷��̾ �濡�� ��������.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleLeavePlayer( PlayerPtr player )
{
	if ( player == nullptr )
		return false;
	
	const uint64 objectId = player->objectInfo->id();
	bool success = _RemoveObject( objectId );
	
	// ���� ����� �����ϴ� �÷��̾�� �˸���
	{
		Protocol::S_LeaveGame leaveGamePkt;
	
		if ( auto session = player->session.lock() )
			session->Send( leaveGamePkt );
	}
	
	// ���� ����� �˸���
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
// @breif �÷��̾��� �������� ó���Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::HandlePlayerMove( Protocol::C_Move pkt )
{
	const AtInt64 id = pkt.objectinfo().id();
	if ( m_objects.find( id ) == m_objects.end() )
		return;

	// TODO : ��ġ üũ�ϱ�
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
// @breif �÷��̾���� ��ȸ�Ѵ�.
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
// @breif �÷��̾�� ��ũ�Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::SyncPlayers( PlayerPtr enterPlayer )
{
	if ( !enterPlayer )
		return;

	// ���� ����� �ٸ� �÷��̾�� �˸���
	{
		Protocol::S_Spawn spawnPkt;

		Protocol::ObjectInfo* playerInfo = spawnPkt.add_objectlist();
		playerInfo->CopyFrom( *enterPlayer->objectInfo );

		Broadcast( spawnPkt, enterPlayer->objectInfo->id() );
	}

	// ���� ������ �÷��̾� ����� ���� �÷��̾����� �������ش�
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
// @breif Room��ü�� ��ȯ�Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
RoomPtr Room::GetPtr()
{
	return static_pointer_cast<Room>( shared_from_this() );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room��ü�� ��ȯ�Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::UpdateTick()
{
	DoAsync( &Room::UpdateTick );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ���� ��� �������� ��ε� ĳ���� �Ѵ�.
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
// @breif ������Ʈ�� �߰��Ѵ�.
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
// @breif ������Ʈ�� �����Ѵ�.
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