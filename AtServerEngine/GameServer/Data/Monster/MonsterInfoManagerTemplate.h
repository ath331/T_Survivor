////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterInfoManagerManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Log/AtLog.h"
#include "MonsterInfo.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class MonsterInfoManagerTemplate
{
protected:
	/// MonsterInfoMap
	using MonsterInfoMap = std::map< AtInt32, MonsterInfo >;

protected:
	/// MonsterInfoMap
	MonsterInfoMap m_infoMap;

public:
	/// GetInfo
	const MonsterInfo* GetInfo( AtInt32 id );

private:
	/// AddInfo
	AtBool _AddInfo(
	    AtInt32 id,
	    AtString string,
	    AtInt32 defaultHP,
	    AtInt32 defaultAttack );

protected:
	/// Initialize
	AtBool _Initialize();
};