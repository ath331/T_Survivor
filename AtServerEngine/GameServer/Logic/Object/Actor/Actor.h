////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Actor class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Object/Object.h"


class GameSession;
class Room;


class Actor
	:
public Object
{
public:
	/// ������
	Actor();

	/// �Ҹ���
	virtual ~Actor();

protected:
	/// ���� Ÿ��
	Protocol::EActorType m_actorType;
};

// ������ Ÿ�� ����
USING_SHARED_PTR( Actor );