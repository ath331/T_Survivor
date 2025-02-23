#pragma once

enum class EPacketId : unsigned __int16
{
	PKT_C_Login = 1000,
	PKT_S_Login = 1001,
	PKT_C_EnterLobby = 1002,
	PKT_S_EnterLobby = 1003,
	PKT_C_EnterGame = 1004,
	PKT_S_EnterGame = 1005,
	PKT_C_EnterGameFinish = 1006,
	PKT_S_EnterGameFinish = 1007,
	PKT_C_LeaveGame = 1008,
	PKT_S_LeaveGame = 1009,
	PKT_C_Move = 1010,
	PKT_S_Move = 1011,
	PKT_S_Spawn = 1012,
	PKT_S_DeSpawn = 1013,
	PKT_C_Chat = 1014,
	PKT_S_Chat = 1015,
	PKT_C_WaitingRoomEnter = 1016,
	PKT_S_WaitingRoomEnter = 1017,
};