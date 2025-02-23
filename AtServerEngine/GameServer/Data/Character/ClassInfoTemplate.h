////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ClassInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "Packet/Protocol.pb.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ClassInfoTemplate class
////////////////////////////////////////////////////////////////////////////////////////////////////
class ClassInfoTemplate
{
public:
	/// Constructor
	ClassInfoTemplate();

	/// Constructor
	ClassInfoTemplate(
	    Protocol::EPlayerType id,
	    AtInt32 hP,
	    AtInt32 mP,
	    AtInt32 damage,
	    AtInt32 magicDamage );

	/// Destructor
	~ClassInfoTemplate();

public:
	/// GetId.
	Protocol::EPlayerType GetId() { return m_id; }

	/// GetHP.
	AtInt32 GetHP() { return m_hP; }

	/// GetMP.
	AtInt32 GetMP() { return m_mP; }

	/// GetDamage.
	AtInt32 GetDamage() { return m_damage; }

	/// GetMagicDamage.
	AtInt32 GetMagicDamage() { return m_magicDamage; }

	/// SetId.
	AtVoid SetId( Protocol::EPlayerType id ) { m_id = id; }

	/// SetHP.
	AtVoid SetHP( AtInt32 hP ) { m_hP = hP; }

	/// SetMP.
	AtVoid SetMP( AtInt32 mP ) { m_mP = mP; }

	/// SetDamage.
	AtVoid SetDamage( AtInt32 damage ) { m_damage = damage; }

	/// SetMagicDamage.
	AtVoid SetMagicDamage( AtInt32 magicDamage ) { m_magicDamage = magicDamage; }


protected:
	/// id
	Protocol::EPlayerType m_id;

	/// hP
	AtInt32 m_hP;

	/// mP
	AtInt32 m_mP;

	/// damage
	AtInt32 m_damage;

	/// magicDamage
	AtInt32 m_magicDamage;
};