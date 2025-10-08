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
private:
	/// AIMove가 업데이트 되었는지 확인하는 변수
	AtBool m_isMoveUpdate;

public:
	/// 생성자
	Actor();

	/// 소멸자
	~Actor() override;

	/// AIMove가 업데이트 변수를 반환한다.
	AtBool GetIsMoveUpdate() const { return m_isMoveUpdate; }

	/// AIMove 업데이트를 변수를 세팅한다.
	AtVoid SetIsMoveUpdate( AtBool value ) { m_isMoveUpdate = value; }

protected:
	/// 엑터 타입
	Protocol::EActorType m_actorType;
};
