////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "JobQueue.h"
#include "CoreMacro.h"
#include "Logic/Room/RoomTypes.h"
#include "Logic/Object/Actor/Player/PlayerTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Room class
////////////////////////////////////////////////////////////////////////////////////////////////////
class Room 
	: 
	public JobQueue
{
public:
	/// 콜백함수 타입 정의
	using CallbackFunc = std::function< AtVoid() >;

	/// 플레이어를 받는 콜백함수 타입 정의
	using CallbackPlayer = std::function< AtVoid( PlayerPtr ) >;

private:
	/// 방 번호
	static std::atomic< AtInt32 > roomNum;

protected:
	/// Lock
	USE_LOCK;

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
	virtual AtBool HandleEnterPlayer( PlayerPtr player, CallbackFunc callback = nullptr );

	/// 플레이어를 방에서 내보낸다.
	AtBool HandleLeavePlayer( PlayerPtr player );

	/// 플레이어의 움직임을 처리한다.
	AtVoid HandlePlayerMove( Protocol::C_Move pkt );

	/// 채팅을 브로드 캐스팅 한다.
	AtVoid BroadcastChat( PlayerPtr sender, Protocol::C_Chat chat );

	/// 플레이어들을 순회한다.
	AtVoid ForeachPlayer( CallbackPlayer callback, AtInt64 exceptId = 0 );

	/// 플레이어끼리 싱크한다.
	AtVoid SyncPlayers( PlayerPtr enterPlayer );

	/// 룸 넘버를 반환한다.
	AtInt32 GetRoomNum() { return roomNum; }

public:
	/// Room객체를 반환한다.
	RoomPtr GetPtr();

public:
	/// 룸을 업데이트한다.
	virtual AtVoid UpdateTick();

	/// 룸의 모든 유저에게 브로드 캐스팅 한다.
	AtVoid Broadcast( google::protobuf::Message& pkt, uint64 exceptId = 0 );

protected:
	/// 오브젝트를 추가한다.
	AtBool _AddObject( ObjectPtr object );

	/// 오브젝트를 제거한다.
	AtBool _RemoveObject( uint64 objectId );

protected:
	/// 모든 오브젝트 맵
	unordered_map<uint64, ObjectPtr > m_objects;

	/// 플레이어 맵
	unordered_map<uint64, PlayerPtr > m_players;
};
