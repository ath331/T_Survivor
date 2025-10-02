////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Monster class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Logic/Object/Actor/Actor.h"
#include "Logic/Object/Actor/ActorTypes.h"


class AI;
class PlayRoom;
class MonsterInfo;


class Monster
	:
	public Actor
{
public:
	/// 생성자
	Monster( AtInt32 monsterInfoId, AtInt32 aIInfoId, PlayRoom* playRoom );

	/// 소멸자
	virtual ~Monster();

	/// 몬스터 정보를 반환한다.
	const MonsterInfo* GetInfo();

private:
	/// 몬스터 정보
	const MonsterInfo* m_monsterInfo;

	/// BT 정보
	AI* m_ai;

	/// 현재 있는 룸 정보
	PlayRoom* m_room;

public:
	/// 업데이트
	AtVoid Update( Millisecond curTime );

private:
	/// 소환시 최초 이동 경로를 반환한다.
	AtVoid _GetFirstMovePath( set< AtInt32 >& movePath );
};
