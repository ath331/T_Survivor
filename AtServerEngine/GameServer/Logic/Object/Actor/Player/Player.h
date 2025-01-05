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
	/// ������
	Player();

	/// �Ҹ���
	virtual ~Player();

	/// session�� �����Ѵ�.
	AtVoid SetSession( weak_ptr< GameSession > session );


public:
	/// ��Ŷ�� �����Ѵ�.
	AtVoid Send( const google::protobuf::Message& pkt );

public:
	/// ���� ����
	weak_ptr< GameSession > session;
};
