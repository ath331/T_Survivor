////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ItemInfoManagerManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Log/AtLog.h"
#include "ItemInfo.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ItemInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class ItemInfoManagerTemplate
{
protected:
	/// ItemInfoMap
	using ItemInfoMap = std::map< AtInt32, ItemInfo >;

protected:
	/// ItemInfoMap
	ItemInfoMap m_infoMap;

public:
	/// GetInfo
	const ItemInfo* GetInfo( AtInt32 id );

private:
	/// AddInfo
	AtBool _AddInfo(
	    AtInt32 id,
	    Protocol::EPlayerType classType,
	    AtString name,
	    AtInt32 level,
	    Protocol::EEquipSlotType equipSlotType,
	    Protocol::EStat stat,
	    AtInt32 statParam );

protected:
	/// Initialize
	AtBool _Initialize();
};