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
    /// 상태 체크 함수 정의
    using ConditionFunc = std::function< AtBool( Actor* ) >;

private:
    /// 상태 체크 함수
    ConditionFunc m_conditionFunc;

public:
    /// 생성자
    AICondition( ConditionFunc conditionFunc );

    /// 실행한다
    virtual AIStatus Execute( Actor* actor ) override;
};
