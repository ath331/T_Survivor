#pragma once

enum class EPacketId : unsigned __int16
{
	PKT_CT_Login = 1000,
	PKT_ST_Login = 1001,
	PKT_CT_EnterLobby = 1002,
	PKT_ST_EnterLobby = 1003,
	PKT_CT_ServerListRead = 1004,
	PKT_ST_ServerListRead = 1005,
};