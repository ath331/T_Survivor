////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterSpawnInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterSpawnInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class MonsterSpawnInfoTemplate
{
public:
	/// Constructor
	MonsterSpawnInfoTemplate();

	/// Constructor
	MonsterSpawnInfoTemplate(
	    AtInt32 id,
	    AtInt32 monsterInfoId,
	    AtInt32 aIInfoId );

	/// Destructor
	~MonsterSpawnInfoTemplate();

public:
	/// GetId.
	AtInt32 GetId() { return m_id; }

	/// GetMonsterInfoId.
	AtInt32 GetMonsterInfoId() { return m_monsterInfoId; }

	/// GetAIInfoId.
	AtInt32 GetAIInfoId() { return m_aIInfoId; }

	/// SetId.
	AtVoid SetId( AtInt32 id ) { m_id = id; }

	/// SetMonsterInfoId.
	AtVoid SetMonsterInfoId( AtInt32 monsterInfoId ) { m_monsterInfoId = monsterInfoId; }

	/// SetAIInfoId.
	AtVoid SetAIInfoId( AtInt32 aIInfoId ) { m_aIInfoId = aIInfoId; }


protected:
	/// id
	AtInt32 m_id;

	/// monsterInfoId
	AtInt32 m_monsterInfoId;

	/// aIInfoId
	AtInt32 m_aIInfoId;
};