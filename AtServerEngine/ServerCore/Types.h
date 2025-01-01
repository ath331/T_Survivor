#pragma once
#include <mutex>
#include <atomic>
#include "CoreMacro.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief CoreType
////////////////////////////////////////////////////////////////////////////////////////////////////
using BYTE = unsigned char;
using int8 = __int8;
using int16 = __int16;
using int32 = __int32;
using int64 = __int64;
using uint8 = unsigned __int8;
using uint16 = unsigned __int16;
using uint32 = unsigned __int32;
using uint64 = unsigned __int64;

////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ContentsType
////////////////////////////////////////////////////////////////////////////////////////////////////
using AtVoid = void;
using AtBool = bool;

using AtBYTE = BYTE;
using AtInt8 = int8;
using AtInt16 = int16;
using AtInt32 = int32;
using AtInt64 = int64;
using AtUInt8 = uint8;
using AtUInt16 = uint16;
using AtUInt32 = uint32;
using AtUInt64 = uint64;
using AtString = std::string;


template<typename T>
using Atomic = std::atomic<T>;
using Mutex = std::mutex;
using CondVar = std::condition_variable;
using UniqueLock = std::unique_lock<std::mutex>;
using LockGuard = std::lock_guard<std::mutex>;

// shared_ptr

USING_SHARED_PTR(IocpCore);
USING_SHARED_PTR(IocpObject);
USING_SHARED_PTR(Session);
USING_SHARED_PTR(PacketSession);
USING_SHARED_PTR(Listener);
USING_SHARED_PTR(ServerService);
USING_SHARED_PTR(ClientService);
USING_SHARED_PTR(SendBuffer);
USING_SHARED_PTR(SendBufferChunk);
USING_SHARED_PTR(Job);
USING_SHARED_PTR(JobQueue);

#define size16(val)		static_cast<int16>(sizeof(val))
#define size32(val)		static_cast<int32>(sizeof(val))
#define len16(arr)		static_cast<int16>(sizeof(arr)/sizeof(arr[0]))
#define len32(arr)		static_cast<int32>(sizeof(arr)/sizeof(arr[0]))

#define _STOMP