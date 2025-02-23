////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ItemInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ItemInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class ItemInfoTemplate
{
public:
	/// Constructor
	ItemInfoTemplate();

	/// Constructor
	ItemInfoTemplate(
	    AtInt32 id,
	    Protocol::EPlayerType classType,
	    AtString name,
	    AtInt32 level,
	    Protocol::EEquipSlotType equipSlotType,
	    Protocol::EStat stat,
	    AtInt32 statParam );

	/// Destructor
	~ItemInfoTemplate();

public:
	/// GetId.
	AtInt32 GetId() { return m_id; }

	/// GetClassType.
	Protocol::EPlayerType GetClassType() { return m_classType; }

	/// GetName.
	AtString GetName() { return m_name; }

	/// GetLevel.
	AtInt32 GetLevel() { return m_level; }

	/// GetEquipSlotType.
	Protocol::EEquipSlotType GetEquipSlotType() { return m_equipSlotType; }

	/// GetStat.
	Protocol::EStat GetStat() { return m_stat; }

	/// GetStatParam.
	AtInt32 GetStatParam() { return m_statParam; }

	/// SetId.
	AtVoid SetId( AtInt32 id ) { m_id = id; }

	/// SetClassType.
	AtVoid SetClassType( Protocol::EPlayerType classType ) { m_classType = classType; }

	/// SetName.
	AtVoid SetName( AtString name ) { m_name = name; }

	/// SetLevel.
	AtVoid SetLevel( AtInt32 level ) { m_level = level; }

	/// SetEquipSlotType.
	AtVoid SetEquipSlotType( Protocol::EEquipSlotType equipSlotType ) { m_equipSlotType = equipSlotType; }

	/// SetStat.
	AtVoid SetStat( Protocol::EStat stat ) { m_stat = stat; }

	/// SetStatParam.
	AtVoid SetStatParam( AtInt32 statParam ) { m_statParam = statParam; }


protected:
	/// id
	AtInt32 m_id;

	/// classType
	Protocol::EPlayerType m_classType;

	/// name
	AtString m_name;

	/// level
	AtInt32 m_level;

	/// equipSlotType
	Protocol::EEquipSlotType m_equipSlotType;

	/// stat
	Protocol::EStat m_stat;

	/// statParam
	AtInt32 m_statParam;
};