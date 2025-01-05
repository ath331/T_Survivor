////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Player class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Object/Actor/Actor.h"


class Player
	:
public Actor
{
public:
	/// 생성자
	Player();

	/// 소멸자
	virtual ~Player();

	/// session을 세팅한다.
	AtVoid SetSession( weak_ptr< GameSession > session );


public:
	/// 패킷을 전송한다.
	AtVoid Send( const google::protobuf::Message& pkt );

public:
	/// 세션 정보
	weak_ptr< GameSession > session;
};
