////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "Room.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Session/GameSession.h"
#include "Packet/Handler/ClientPacketHandler.h"


RoomPtr GRoom = make_shared< Room >();


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::Room()
{

}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �Ҹ���
////////////////////////////////////////////////////////////////////////////////////////////////////
Room::~Room()
{

}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������Ʈ�� �����Ų��.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::EnterRoom( ObjectPtr object, AtBool randPos )
{
	AtBool success = AddObject( object );

	if ( randPos )
	{
		object->posInfo->set_x  ( Utils::GetRandom( 0.f, 500.f ) );
		object->posInfo->set_y  ( Utils::GetRandom( 0.f, 500.f ) );
		// object->posInfo->set_z  ( Utils::GetRandom( 0.f, 500.f ) );
		object->posInfo->set_z  ( 100.f );
		object->posInfo->set_yaw( Utils::GetRandom( 0.f, 100.f ) );
	}

	// ���� ����� ���� �÷��̾�� �˸���
	if ( auto player = dynamic_pointer_cast<Player>( object ) )
	{
		Protocol::S_EnterGame enterGamePkt;
		enterGamePkt.set_success( success );

		Protocol::ObjectInfo* playerInfo = new Protocol::ObjectInfo();
		playerInfo->CopyFrom( *player->objectInfo );
		enterGamePkt.set_allocated_player( playerInfo );
		//enterGamePkt.release_player();

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( enterGamePkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	// ���� ����� �ٸ� �÷��̾�� �˸���
	{
		Protocol::S_Spawn spawnPkt;

		Protocol::ObjectInfo* objectInfo = spawnPkt.add_players();
		objectInfo->CopyFrom( *object->objectInfo );

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( spawnPkt );
		Broadcast( sendBuffer, object->objectInfo->id() );
	}

	// ���� ������ �÷��̾� ����� ���� �÷��̾����� �������ش�
	if ( auto player = dynamic_pointer_cast<Player>( object ) )
	{
		Protocol::S_Spawn spawnPkt;

		for ( auto& item : m_objects )
		{
			if ( !item.second->IsPlayer() )
				continue;

			Protocol::ObjectInfo* playerInfo = spawnPkt.add_players();
			playerInfo->CopyFrom( *item.second->objectInfo );
		}

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( spawnPkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������Ʈ�� �����Ų��.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::LeaveRoom( ObjectPtr object )
{
	if ( !object )
		return false;

	const uint64 objectId = object->objectInfo->id();
	bool success = RemoveObject( objectId );

	// ���� ����� �����ϴ� �÷��̾�� �˸���
	if ( auto player = dynamic_pointer_cast<Player>( object ) )
	{
		Protocol::S_LeaveGame leaveGamePkt;

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( leaveGamePkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	// ���� ����� �˸���
	{
		Protocol::S_DeSpawn despawnPkt;
		despawnPkt.add_ids( objectId );

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( despawnPkt );
		Broadcast( sendBuffer, objectId );

		if ( auto player = dynamic_pointer_cast<Player>( object ) )
		{
			if ( auto session = player->session.lock() )
				session->Send( sendBuffer );
		}
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �÷��̾ �濡 �����Ų��.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleEnterPlayer( PlayerPtr player )
{
	AtBool success = AddObject( player );

	// ���� ��ġ
	player->posInfo->set_x  ( Utils::GetRandom( 0.f, 500.f ) );
	player->posInfo->set_y  ( Utils::GetRandom( 0.f, 500.f ) );
	//player->playerInfo->set_z  ( Utils::GetRandom( 0.f, 500.f ) );
	player->posInfo->set_z  ( 100.f );
	player->posInfo->set_yaw( Utils::GetRandom( 0.f, 100.f ) );

	// ���� ����� ���� �÷��̾�� �˸���
	{
		Protocol::S_EnterGame enterGamePkt;
		enterGamePkt.set_success( success );

		Protocol::ObjectInfo* playerInfo = new Protocol::ObjectInfo();
		playerInfo->CopyFrom( *player->objectInfo );
		enterGamePkt.set_allocated_player( playerInfo );
		//enterGamePkt.release_player();

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( enterGamePkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	// ���� ����� �ٸ� �÷��̾�� �˸���
	{
		Protocol::S_Spawn spawnPkt;

		Protocol::ObjectInfo* playerInfo = spawnPkt.add_players();
		playerInfo->CopyFrom( *player->objectInfo );

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( spawnPkt );
		Broadcast( sendBuffer, player->objectInfo->id() );
	}

	// ���� ������ �÷��̾� ����� ���� �÷��̾����� �������ش�
	{
		Protocol::S_Spawn spawnPkt;

		for ( auto& item : m_objects )
		{
			if ( !item.second->IsPlayer() )
				continue;

			Protocol::ObjectInfo* playerInfo = spawnPkt.add_players();
			playerInfo->CopyFrom( *item.second->objectInfo );
		}

		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( spawnPkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �÷��̾ �濡�� ��������.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleLeavePlayer( PlayerPtr player )
{
	if ( player == nullptr )
		return false;
	
	const uint64 objectId = player->objectInfo->id();
	bool success = RemoveObject( objectId );
	
	// ���� ����� �����ϴ� �÷��̾�� �˸���
	{
		Protocol::S_LeaveGame leaveGamePkt;
	
		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( leaveGamePkt );
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}
	
	// ���� ����� �˸���
	{
		Protocol::S_DeSpawn despawnPkt;
		despawnPkt.add_ids( objectId );
	
		SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( despawnPkt );
		Broadcast( sendBuffer, objectId );
	
		if ( auto session = player->session.lock() )
			session->Send( sendBuffer );
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �÷��̾��� �������� ó���Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::HandlePlayerMove( Protocol::C_Move pkt )
{
	const AtInt64 id = pkt.info().id();
	if ( m_objects.find( id ) == m_objects.end() )
		return;

	// TODO : ��ġ üũ�ϱ�
	PlayerPtr player = dynamic_pointer_cast<Player>( m_objects[ id ] );
	if ( !player )
		return;

	player->posInfo->CopyFrom( pkt.info() );

	Protocol::S_Move movePkt;
	auto* info =  movePkt.mutable_info();
	info->CopyFrom( pkt.info() );

	SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( movePkt );
	Broadcast( sendBuffer, id );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room��ü�� ��ȯ�Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::UpdateTick()
{
	// cout << "Room Tick" << endl;
	// 
	// DoTimer( 100, &Room::UpdateTick );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room��ü�� ��ȯ�Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
RoomPtr Room::GetPtr()
{
	return static_pointer_cast<Room>( shared_from_this() );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������Ʈ�� �߰��Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::AddObject( ObjectPtr object )
{
	if ( m_objects.find( object->objectInfo->id() ) != m_objects.end() )
		return false;

	m_objects.insert( make_pair( object->objectInfo->id(), object ) );

	object->room.store( GetPtr() );

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������Ʈ�� �����Ѵ�.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::RemoveObject( uint64 objectId )
{
	if ( m_objects.find( objectId ) == m_objects.end() )
		return false;

	ObjectPtr object = m_objects[ objectId ];
	if ( !object )
		return false;

	object->room.store( weak_ptr<Room>() );

	m_objects.erase( objectId );

	return true;
}

AtVoid Room::Broadcast( SendBufferPtr sendBuffer, uint64 exceptId )
{
	for ( auto& item : m_objects )
	{
		PlayerPtr player = dynamic_pointer_cast<Player>( item.second );
		if ( !player )
			continue;

		if ( player->objectInfo->id() == exceptId )
			continue;

		if ( GameSessionPtr session = player->session.lock() )
			session->Send( sendBuffer );
	}
}