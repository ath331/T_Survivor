////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief InfoManagers ( Auto Make File )
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "pch.h"
#include <iostream>
#include "Logic/Utils/Log/AtLog.h"
#include "Data/TestInfoManager.h"
#include "Data/Character/CharacterInfoManager.h"
#include "Data/Character/ClassInfoManager.h"
#include "Data/Inventory/InventoryInfoManager.h"
#include "Data/Item/ItemInfoManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief InfoManagers
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool InitializeInfoManager()
{
	INFO_LOG( "InitializeInfoManager Start" );
	if ( !TestInfoManager::GetInstance().Initialize() ) return false;
	if ( !CharacterInfoManager::GetInstance().Initialize() ) return false;
	if ( !ClassInfoManager::GetInstance().Initialize() ) return false;
	if ( !InventoryInfoManager::GetInstance().Initialize() ) return false;
	if ( !ItemInfoManager::GetInstance().Initialize() ) return false;


	INFO_LOG( "InitializeInfoManager End" );

	return true;
}


