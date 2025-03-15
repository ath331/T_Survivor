////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AICondition class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AINode.h"


class AICondition
	:
	public AINode
{
private:
    /// ���� üũ �Լ� ����
    using ConditionFunc = std::function< AtBool( Actor* ) >;

private:
    /// ���� üũ �Լ�
    ConditionFunc m_conditionFunc;

public:
    /// ������
    AICondition( ConditionFunc conditionFunc );

    /// �����Ѵ�
    virtual AIStatus Execute( Actor* actor ) override;
};
