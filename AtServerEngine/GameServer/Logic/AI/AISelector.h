////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AISelector class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AINode.h"


class AISelector : public AINode
{
private:
	std::vector< AINode* > children;

public:
	/// ������
	AISelector( const std::vector< AINode* >& nodes );

	/// �Ҹ���
	virtual ~AISelector();

	/// �����Ѵ�.
	virtual AIStatus Execute( Actor* actor ) override;
};
