////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief MonsterInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class MonsterInfoTemplate
{
public:
	/// Constructor
	MonsterInfoTemplate();

	/// Constructor
	MonsterInfoTemplate(
	    AtInt32 id,
	    AtString string,
	    AtInt32 defaultHP,
	    AtInt32 defaultAttack );

	/// Destructor
	~MonsterInfoTemplate();

public:
	/// GetId.
	AtInt32 GetId() { return m_id; }

	/// GetString.
	AtString GetString() { return m_string; }

	/// GetDefaultHP.
	AtInt32 GetDefaultHP() { return m_defaultHP; }

	/// GetDefaultAttack.
	AtInt32 GetDefaultAttack() { return m_defaultAttack; }

	/// SetId.
	AtVoid SetId( AtInt32 id ) { m_id = id; }

	/// SetString.
	AtVoid SetString( AtString string ) { m_string = string; }

	/// SetDefaultHP.
	AtVoid SetDefaultHP( AtInt32 defaultHP ) { m_defaultHP = defaultHP; }

	/// SetDefaultAttack.
	AtVoid SetDefaultAttack( AtInt32 defaultAttack ) { m_defaultAttack = defaultAttack; }


protected:
	/// id
	AtInt32 m_id;

	/// string
	AtString m_string;

	/// defaultHP
	AtInt32 m_defaultHP;

	/// defaultAttack
	AtInt32 m_defaultAttack;
};