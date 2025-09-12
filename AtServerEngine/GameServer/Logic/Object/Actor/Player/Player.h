////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Player class
////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Object/Actor/Actor.h"
#include "Logic/ContentsManager.h"
#include "Ptr/PtrProtectContainer.h"


class Inventory;


class Player
	:
	public Actor
{
private:
	/// 안전한 해제를 위한 포인터 컨테이너
	PtrProtectContainer< ContentsManager > m_contentsManagerContainer;

	/// Inventory
	Inventory* m_inventory = nullptr;

public:
	/// 생성자
	Player();

	/// 소멸자
	~Player() override;

	/// 현재 룸을 반환한다.
	RoomPtr GetRoomPtr();

	/// 패킷을 전송한다.
	AtVoid Send( google::protobuf::Message& pkt );

/// ContentsManager 공통 로직
public:
	/// 로그인시 처리한다.
	AtVoid OnLogin();

public:
	/// 세션 정보
	weak_ptr< GameSession > session;
};
