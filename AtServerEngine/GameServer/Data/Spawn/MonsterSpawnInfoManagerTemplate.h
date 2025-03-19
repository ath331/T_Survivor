////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterSpawnInfoManagerManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Log/AtLog.h"
#include "MonsterSpawnInfo.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterSpawnInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class MonsterSpawnInfoManagerTemplate
{
protected:
	/// MonsterSpawnInfoMap
	using MonsterSpawnInfoMap = std::map< AtInt32, MonsterSpawnInfo >;

protected:
	/// MonsterSpawnInfoMap
	MonsterSpawnInfoMap m_infoMap;

public:
	/// GetInfo
	const MonsterSpawnInfo* GetInfo( AtInt32 id );

private:
	/// AddInfo
	AtBool _AddInfo(
	    AtInt32 id,
	    AtInt32 groupId,
	    AtInt32 monsterInfoId,
	    AtInt32 aIInfoId );

protected:
	/// Initialize
	AtBool _Initialize();
};