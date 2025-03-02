////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "Room.h"
#include "Logic/Object/Actor/Player/Player.h"
#include "Logic/Utils/Log/AtLog.h"
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
// @breif 오브젝트를 입장시킨다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::EnterRoom( ObjectPtr object, AtBool randPos )
{
	AtBool success = _AddObject( object );

	if ( randPos )
	{
		object->posInfo->set_x  ( Utils::GetRandom( 0.f, 500.f ) );
		object->posInfo->set_y  ( Utils::GetRandom( 0.f, 500.f ) );
		// object->posInfo->set_z  ( Utils::GetRandom( 0.f, 500.f ) );
		object->posInfo->set_z  ( 100.f );
		object->posInfo->set_yaw( Utils::GetRandom( 0.f, 100.f ) );
	}

	// 입장 사실을 신입 플레이어에게 알린다
	if ( auto player = dynamic_pointer_cast<Player>( object ) )
	{
		Protocol::S_EnterGame enterGamePkt;
		if ( success )
			enterGamePkt.set_result( Protocol::EResultCode::RESULT_CODE_SUCCESS );
		else
			enterGamePkt.set_result( Protocol::EResultCode::RESULT_CODE_FAIL_ROOM_ENTER );

		Protocol::ObjectInfo* playerInfo = new Protocol::ObjectInfo();
		playerInfo->CopyFrom( *player->objectInfo );
		enterGamePkt.set_allocated_player( playerInfo );
		//enterGamePkt.release_player();

		if ( auto session = player->session.lock() )
			session->Send( enterGamePkt );
	}

	// 입장 사실을 다른 플레이어에게 알린다
	{
		Protocol::S_Spawn spawnPkt;

		Protocol::ObjectInfo* objectInfo = spawnPkt.add_players();
		objectInfo->CopyFrom( *object->objectInfo );

		_Broadcast( spawnPkt, object->objectInfo->id() );
	}

	// 기존 입장한 플레이어 목록을 신입 플레이어한테 전송해준다
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

		if ( auto session = player->session.lock() )
			session->Send( spawnPkt );
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 오브젝트를 퇴장시킨다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::LeaveRoom( ObjectPtr object )
{
	if ( !object )
		return false;

	const uint64 objectId = object->objectInfo->id();
	bool success = _RemoveObject( objectId );

	// 퇴장 사실을 퇴장하는 플레이어에게 알린다
	if ( auto player = dynamic_pointer_cast<Player>( object ) )
	{
		Protocol::S_LeaveGame leaveGamePkt;

		if ( auto session = player->session.lock() )
			session->Send( leaveGamePkt );
	}

	// 퇴장 사실을 알린다
	{
		Protocol::S_DeSpawn despawnPkt;
		despawnPkt.add_ids( objectId );

		_Broadcast( despawnPkt, objectId );

		if ( auto player = dynamic_pointer_cast<Player>( object ) )
		{
			if ( auto session = player->session.lock() )
				session->Send( despawnPkt );
		}
	}

	return success;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어를 방에 입장시킨다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::HandleEnterPlayer( PlayerPtr player, CallbackFunc callback )
{
	AtBool success = _AddObject( player );

	// TODO : S_EnterGame으로 옮기기 
	// 랜덤 위치
	player->posInfo->set_x  ( Utils::GetRandom( 0.f, 500.f ) );
	player->posInfo->set_y  ( Utils::GetRandom( 0.f, 500.f ) );
	//player->playerInfo->set_z  ( Utils::GetRandom( 0.f, 500.f ) );
	player->posInfo->set_z  ( 100.f );
	player->posInfo->set_yaw( Utils::GetRandom( 0.f, 100.f ) );

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
	
		_Broadcast( despawnPkt, objectId );
	
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
	const AtInt64 id = pkt.info().id();
	if ( m_objects.find( id ) == m_objects.end() )
		return;

	// TODO : 위치 체크하기
	PlayerPtr player = dynamic_pointer_cast<Player>( m_objects[ id ] );
	if ( !player )
		return;

	player->posInfo->CopyFrom( pkt.info() );

	Protocol::S_Move movePkt;
	auto* info =  movePkt.mutable_info();
	info->CopyFrom( pkt.info() );

	_Broadcast( movePkt, id );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 채팅을 브로드 캐스팅 한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::BroadcastChat( PlayerPtr sender, Protocol::C_Chat chat )
{
	Protocol::S_Chat chatResult;
	chatResult.set_playerid( sender->GetId() );
	chatResult.set_msg( chat.msg() );

	_Broadcast( chatResult );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 플레이어들을 순회한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::ForeachPlayer( CallbackPlayer callback, AtInt64 exceptId )
{
	// TODO : 모든 오브젝트를 순회할 필요는 없으니 플레이어 컨테이너를 따로 만들기
	for ( const auto& object : m_objects )
	{
		ObjectPtr objectPtr = object.second;
		if ( !objectPtr )
			continue;

		PlayerPtr player = dynamic_pointer_cast<Player>( objectPtr );
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

		Protocol::ObjectInfo* playerInfo = spawnPkt.add_players();
		playerInfo->CopyFrom( *enterPlayer->objectInfo );

		_Broadcast( spawnPkt, enterPlayer->objectInfo->id() );
	}

	// 기존 입장한 플레이어 목록을 신입 플레이어한테 전송해준다
	{
		Protocol::S_Spawn spawnPkt;

		for ( auto& item : m_objects )
		{
			if ( !item.second->IsPlayer() )
				continue;

			Protocol::ObjectInfo* playerInfo = spawnPkt.add_players();
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
// @breif 오브젝트를 추가한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::_AddObject( ObjectPtr object )
{
	if ( m_objects.find( object->objectInfo->id() ) != m_objects.end() )
		return false;

	m_objects.insert( make_pair( object->objectInfo->id(), object ) );

	object->room.store( GetPtr() );

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 오브젝트를 제거한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool Room::_RemoveObject( uint64 objectId )
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

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 룸의 모든 유저에게 브로드 캐스팅 한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Room::_Broadcast( google::protobuf::Message& pkt, uint64 exceptId )
{
	SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( pkt );

	for ( auto& item : m_objects )
	{
		PlayerPtr player = dynamic_pointer_cast<Player>( item.second );
		if ( !player )
			continue;

		if ( player->objectInfo->id() == exceptId )
			continue;

		if ( GameSessionPtr session = player->session.lock() )
			session->Send( pkt );
	}
}