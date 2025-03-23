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
	/// �ִ� �ο�
	AtInt32 m_maxUserCount;

	/// �� ����
	AtString m_name;

	/// ����� ����
	AtBool m_isPrivate;

	/// ��й�ȣ
	AtInt32 m_pw;

public:
	/// ������
	WaitingRoom(
		AtInt32  maxUserCount = 3,
		AtString name = "Default",
		AtInt32  pw = 0 );

	/// ���� ������Ʈ�Ѵ�.
	AtVoid UpdateTick() override;

	/// ������ ��������.
	AtVoid ExportTo( Protocol::RoomInfo& roomInfo );

	/// ������ ��������.
	AtVoid ExportTo( Protocol::RoomInfo* roomInfo );

	/// �濡 ������ �� �ִ��� Ȯ���Ѵ�.
	AtBool CheckEnterRoom() const;
};
