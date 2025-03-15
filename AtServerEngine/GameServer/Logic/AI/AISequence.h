////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AISequence class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AINode.h"


class AISequence : public AINode
{
private:
	std::vector< AINode* > children;

public:
	/// ������
	AISequence( const std::vector< AINode* >& nodes );

	/// �Ҹ���
	virtual ~AISequence();

	/// �����Ѵ�.
	virtual AIStatus Execute( Actor* actor ) override;
};
