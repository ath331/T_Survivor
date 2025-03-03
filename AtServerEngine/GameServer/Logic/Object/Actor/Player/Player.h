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

	/// ��Ŷ�� �����Ѵ�.
	AtVoid Send( google::protobuf::Message& pkt );

public:
	/// ���� ����
	weak_ptr< GameSession > session;
};
