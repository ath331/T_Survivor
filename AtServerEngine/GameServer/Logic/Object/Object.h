////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Object class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Packet/Protocol.pb.h"


class GameSession;
class Room;


class Object
	:
public enable_shared_from_this< Object >
{
public:
	/// ������
	Object();

	/// �Ҹ���
	virtual ~Object();

	/// �÷��̾� Ÿ������ Ȯ���Ѵ�.
	AtBool IsPlayer() { return m_isPlayer; }

public:
	/// ������Ʈ ����
	Protocol::ObjectInfo* objectInfo;

	// ��ġ ����
	Protocol::PosInfo* posInfo;

	/// �������� Room ����
	atomic< weak_ptr< Room >> room;

protected:
	AtBool m_isPlayer = false;
};
