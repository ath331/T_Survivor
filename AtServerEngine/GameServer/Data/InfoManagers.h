////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief InfoManagers ( Auto Make File )
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "pch.h"
#include <iostream>
#include "Logic/Utils/Log/AtLog.h"
#include "Data/TestInfoManager.h"
#include "Data/AI/AIInfoManager.h"
#include "Data/Character/CharacterInfoManager.h"
#include "Data/Character/ClassInfoManager.h"
#include "Data/Inventory/InventoryInfoManager.h"
#include "Data/Item/ItemInfoManager.h"
#include "Data/Monster/MonsterInfoManager.h"
#include "Data/Spawn/MonsterSpawnInfoManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief InfoManagers
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool InitializeInfoManager()
{
	INFO_LOG( "InitializeInfoManager Start" );
	if ( !TestInfoManager::GetInstance().Initialize() ) return false;
	if ( !AIInfoManager::GetInstance().Initialize() ) return false;
	if ( !CharacterInfoManager::GetInstance().Initialize() ) return false;
	if ( !ClassInfoManager::GetInstance().Initialize() ) return false;
	if ( !InventoryInfoManager::GetInstance().Initialize() ) return false;
	if ( !ItemInfoManager::GetInstance().Initialize() ) return false;
	if ( !MonsterInfoManager::GetInstance().Initialize() ) return false;
	if ( !MonsterSpawnInfoManager::GetInstance().Initialize() ) return false;


	INFO_LOG( "InitializeInfoManager End" );

	return true;
}


