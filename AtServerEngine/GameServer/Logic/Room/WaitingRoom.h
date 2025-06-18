////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoom File
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Room.h"
#include "CoreMacro.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif WaitingRoom class
////////////////////////////////////////////////////////////////////////////////////////////////////
class WaitingRoom
	:
	public Room
{
private:
	/// 최대 인원
	AtInt32 m_maxUserCount;

	/// 방 제목
	AtString m_name;

	/// 비공개 여부
	AtBool m_isPrivate;

	/// 비밀번호
	AtInt32 m_pw;

	/// 현재 상태
	ERoomState m_state;

public:
	/// 생성자
	WaitingRoom(
		AtInt32  maxUserCount = 3,
		AtString name = "Default",
		AtInt32  pw = 0 );

	/// 룸을 업데이트한다.
	AtVoid UpdateTick( Millisecond curTime ) override;

	/// 정보를 내보낸다.
	AtVoid ExportTo( RoomInfo& roomInfo ) override;

	/// 방에 입장할 수 있는지 확인한다.
	AtBool CheckEnterRoom() const;

// override Room
protected:
	/// 플레이어가 방에 입장한 다음 처리한다.
	AtVoid _OnPlayerEnter( PlayerPtr player ) override;
};
