////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ItemInfoManagerTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "pch.h"
#include <iostream>
#include "ItemInfoManagerTemplate.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief GetInfo
////////////////////////////////////////////////////////////////////////////////////////////////////
const ItemInfo* ItemInfoManagerTemplate::GetInfo( AtInt32 id )
{
	auto iter = m_infoMap.find( id );
	if ( iter == m_infoMap.end() )
		return nullptr;

	return &(iter->second);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AddInfo
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool ItemInfoManagerTemplate::_AddInfo(
	    AtInt32 id,
	    Protocol::EPlayerType classType,
	    AtString name,
	    AtInt32 level,
	    Protocol::EEquipSlotType equipSlotType,
	    Protocol::EStat stat,
	    AtInt32 statParam )
{
	auto iter = m_infoMap.find( id );
	if ( iter != m_infoMap.end() )
		return false;

	ItemInfo info = ItemInfo(
	    id,
	    classType,
	    name,
	    level,
	    equipSlotType,
	    stat,
	    statParam );

	m_infoMap[ id ] = info;

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief Initialize
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool ItemInfoManagerTemplate::_Initialize()
{
	INFO_LOG( "ItemInfoManager Initialize()" );
	if ( !_AddInfo( 1000, Protocol::EPlayerType::PLAYER_TYPE_KNIGHT, "���� Į", 1, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_WEAPON, Protocol::EStat::STAT_STRENGTH, 5 ) ) return false;
	if ( !_AddInfo( 1001, Protocol::EPlayerType::PLAYER_TYPE_KNIGHT, "���� ����", 1, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_SUB_WEAPON, Protocol::EStat::STAT_DEFENSE, 10 ) ) return false;
	if ( !_AddInfo( 1002, Protocol::EPlayerType::PLAYER_TYPE_MAX, "���� ����", 3, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_HELMAT, Protocol::EStat::STAT_DEFENSE, 5 ) ) return false;
	if ( !_AddInfo( 1003, Protocol::EPlayerType::PLAYER_TYPE_MAX, "���� ����", 3, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_ARMOR, Protocol::EStat::STAT_DEFENSE, 5 ) ) return false;
	if ( !_AddInfo( 1004, Protocol::EPlayerType::PLAYER_TYPE_MAX, "���� �尩", 3, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_GLOVES, Protocol::EStat::STAT_DEFENSE, 5 ) ) return false;
	if ( !_AddInfo( 1005, Protocol::EPlayerType::PLAYER_TYPE_MAX, "���� �Ź�", 3, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_BOOTS, Protocol::EStat::STAT_SPEED, 15 ) ) return false;
	if ( !_AddInfo( 1006, Protocol::EPlayerType::PLAYER_TYPE_MAGE, "���� ������", 1, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_WEAPON, Protocol::EStat::STAT_INTELLIGENCE, 20 ) ) return false;
	if ( !_AddInfo( 1007, Protocol::EPlayerType::PLAYER_TYPE_MAGE, "���� ����", 1, Protocol::EEquipSlotType::EQUIP_SLOT_TYPE_SUB_WEAPON, Protocol::EStat::STAT_MP, 10 ) ) return false;

	return true;
}