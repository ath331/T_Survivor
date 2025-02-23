////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ClassInfoManagerManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Log/AtLog.h"
#include "ClassInfo.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ClassInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class ClassInfoManagerTemplate
{
protected:
	/// ClassInfoMap
	using ClassInfoMap = std::map< Protocol::EPlayerType, ClassInfo >;

protected:
	/// ClassInfoMap
	ClassInfoMap m_infoMap;

public:
	/// GetInfo
	const ClassInfo* GetInfo( Protocol::EPlayerType id );

private:
	/// AddInfo
	AtBool _AddInfo(
	    Protocol::EPlayerType id,
	    AtInt32 hP,
	    AtInt32 mP,
	    AtInt32 damage,
	    AtInt32 magicDamage );

protected:
	/// Initialize
	AtBool _Initialize();
};