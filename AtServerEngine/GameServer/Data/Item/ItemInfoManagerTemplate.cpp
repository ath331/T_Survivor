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
	if ( !_AddInfo( 1000, Protocol::EPlayerType::PLAYER_TYPE_KNIGHT, "³ª¹« Ä®", 1, Protocol::EEquipSlotType::Weapon, Protocol::EStat::Strength, 5 ) ) return false;
	if ( !_AddInfo( 1001, Protocol::EPlayerType::PLAYER_TYPE_KNIGHT, "³ª¹« ¹æÆÐ", 1, Protocol::EEquipSlotType::SubWeapon, Protocol::EStat::Defense, 10 ) ) return false;
	if ( !_AddInfo( 1002, Protocol::EPlayerType::EPlayerTypeMax, "°¡Á× Åõ±¸", 3, Protocol::EEquipSlotType::Helmet, Protocol::EStat::Defense, 5 ) ) return false;
	if ( !_AddInfo( 1003, Protocol::EPlayerType::EPlayerTypeMax, "°¡Á× °©¿Ê", 3, Protocol::EEquipSlotType::Armor, Protocol::EStat::Defense, 5 ) ) return false;
	if ( !_AddInfo( 1004, Protocol::EPlayerType::EPlayerTypeMax, "°¡Á× Àå°©", 3, Protocol::EEquipSlotType::Gloves, Protocol::EStat::Defense, 5 ) ) return false;
	if ( !_AddInfo( 1005, Protocol::EPlayerType::EPlayerTypeMax, "°¡Á× ½Å¹ß", 3, Protocol::EEquipSlotType::Boots, Protocol::EStat::Speed, 15 ) ) return false;
	if ( !_AddInfo( 1006, Protocol::EPlayerType::PLAYER_TYPE_MAGE, "³ª¹« ÁöÆÎÀÌ", 1, Protocol::EEquipSlotType::Weapon, Protocol::EStat::Intelligence, 20 ) ) return false;
	if ( !_AddInfo( 1007, Protocol::EPlayerType::PLAYER_TYPE_MAGE, "³ª¹« ±¸½½", 1, Protocol::EEquipSlotType::SubWeapon, Protocol::EStat::MP, 10 ) ) return false;

	return true;
}