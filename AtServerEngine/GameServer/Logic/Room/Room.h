////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Object/Actor/Player/PlayerTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room class
////////////////////////////////////////////////////////////////////////////////////////////////////
class Room 
	: 
	public JobQueue
{
private:
	/// 방 번호
	static std::atomic< AtInt32 > roomNum;

public:
	/// 생성자
	Room();

	/// 소멸자
	virtual ~Room();

	/// 오브젝트를 입장시킨다.
	AtBool EnterRoom( ObjectPtr object, AtBool randPos = true );

	/// 오브젝트를 퇴장시킨다.
	AtBool LeaveRoom( ObjectPtr object );

	/// 플레이어를 방에 입장시킨다.
	virtual AtBool HandleEnterPlayer( PlayerPtr player );

	/// 플레이어를 방에서 내보낸다.
	AtBool HandleLeavePlayer( PlayerPtr player );

	/// 플레이어의 움직임을 처리한다.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

	/// 채팅을 브로드 캐스팅 한다.
	AtVoid BroadcastChat( PlayerPtr sender, Protocol::C_Chat chat );

public:
	/// Room객체를 반환한다.
	RoomPtr GetPtr();

public:
	/// 룸을 업데이트한다.
	virtual AtVoid UpdateTick();

protected:
	/// 오브젝트를 추가한다.
	AtBool _AddObject( ObjectPtr object );

	/// 오브젝트를 제거한다.
	AtBool _RemoveObject( uint64 objectId );

	/// 룸의 모든 유저에게 브로드 캐스팅 한다.
	AtVoid _Broadcast( google::protobuf::Message& pkt, uint64 exceptId = 0 );

protected:
	unordered_map<uint64, ObjectPtr > m_objects;
};
