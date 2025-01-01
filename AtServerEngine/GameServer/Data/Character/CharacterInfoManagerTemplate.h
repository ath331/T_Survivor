////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief CharacterInfoManagerManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Log/AtLog.h"
#include "CharacterInfo.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief CharacterInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class CharacterInfoManagerTemplate
{
protected:
	/// CharacterInfoMap
	using CharacterInfoMap = std::map< AtInt32, CharacterInfo >;

protected:
	/// CharacterInfoMap
	CharacterInfoMap m_infoMap;

public:
	/// GetInfo
	const CharacterInfo* GetInfo( AtInt32 id );

private:
	/// AddInfo
	AtBool _AddInfo(
	    AtInt32 id,
	    AtBool isNonUse,
	    Protocol::EBagType bagType );

protected:
	/// Initialize
	AtBool _Initialize();
};