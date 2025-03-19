////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Monster class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Logic/Object/Actor/Actor.h"
#include "Logic/Object/Actor/ActorTypes.h"


class AI;
class MonsterInfo;


class Monster
	:
	public Actor
{
public:
	/// ������
	Monster( AtInt32 monsterInfoId, AtInt32 aIInfoId );

	/// �Ҹ���
	virtual ~Monster();

	/// ���� ������ ��ȯ�Ѵ�.
	const MonsterInfo* GetInfo();

private:
	/// ���� ����
	const MonsterInfo* m_monsterInfo;

	/// BT ����
	AI* m_ai;

public:
	/// ������Ʈ
	AtVoid Update();
};
