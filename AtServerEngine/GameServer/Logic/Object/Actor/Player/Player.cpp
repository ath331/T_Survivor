////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Player class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "Player.h"
#include "Session/GameSession.h"
#include "Logic/Room/Room.h"
#include "Packet/Handler/ClientPacketHandler.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
Player::Player()
{
	m_isPlayer = true;
	m_actorType = Protocol::EActorType::ACTOR_TYPE_PLAYER;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소멸자
////////////////////////////////////////////////////////////////////////////////////////////////////
Player::~Player()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 현재 룸을 반환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
RoomPtr Player::GetRoomPtr()
{
	return room.load().lock();
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 패킷을 전송한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid Player::Send( google::protobuf::Message& pkt )
{
	if ( auto sessionPtr = session.lock() )
		sessionPtr->Send( pkt );
}
