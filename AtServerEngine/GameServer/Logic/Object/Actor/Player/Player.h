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

	/// ���� ���� ��ȯ�Ѵ�.
	RoomPtr GetRoomPtr();

	/// ��Ŷ�� �����Ѵ�.
	AtVoid Send( google::protobuf::Message& pkt );

public:
	/// ���� ����
	weak_ptr< GameSession > session;
};
