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
	/// 생성자
	Monster( AtInt32 monsterInfoId, AtInt32 aIInfoId );

	/// 소멸자
	virtual ~Monster();

	/// 몬스터 정보를 반환한다.
	const MonsterInfo* GetInfo();

private:
	/// 몬스터 정보
	const MonsterInfo* m_monsterInfo;

	/// BT 정보
	AI* m_ai;

public:
	/// 업데이트
	AtVoid Update();
};
