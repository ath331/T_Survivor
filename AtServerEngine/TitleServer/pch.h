#pragma once

#define _CRT_SECURE_NO_WARNINGS // disable C4996 warning

#define WIN32_LEAN_AND_MEAN // 거의 사용되지 않는 내용을 Windows 헤더에서 제외합니다.

#ifdef _DEBUG
#pragma comment(lib, "ServerCore\\Debug\\ServerCore.lib")
#pragma comment(lib, "Public\\Debug\\Public.lib")
#pragma comment(lib, "Network\\Debug\\Network.lib")
#pragma comment(lib, "Protobuf\\Debug\\libprotobufd.lib")
#else
#pragma comment(lib, "ServerCore\\Release\\ServerCore.lib")
#pragma comment(lib, "Public\\Release\\Public.lib")
#pragma comment(lib, "Network\\Release\\Network.lib")
#pragma comment(lib, "Protobuf\\Release\\libprotobuf.lib")
#endif

#include "../ServerCore/CorePch.h"
#include "Packet/Handler/ClientPacketHandler.h"
#include "Logic/Utils/Utils.h"
#include "Logic/Utils/String/StringUtils.h"
#include "Logic/Utils/Log/AtLog.h"
#include "Logic/Core/Environment.h"
#include "DB/DBConnectionGaurd.h"
#include "DB/GenProcedures.h"


//#include "Packet/Enum.pb.h"
#include "Packet/Protocol.pb.h"
//#include "Packet/Struct.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief Ptr
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "CoreMacro.h"
#include "Session/TitleSession.h"
#include "Session/TitleSessionTypes.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief Packet
////////////////////////////////////////////////////////////////////////////////////////////////////
#define SEND_PACKET( session, pkt )  \
	SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( pkt ); \
	session->Send( sendBuffer );


using namespace Protocol;


using Second = std::chrono::duration<int64_t, std::ratio<1>>;
using Millisecond = std::chrono::duration<AtInt64, std::milli>;