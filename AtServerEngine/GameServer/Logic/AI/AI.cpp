////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AI class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "AI.h"
#include "AINode.h"
#include "AISelector.h"
#include "AISequence.h"
#include "AIAttackAction.h"
#include "AIChaseAction.h"
#include "AIMoveAction.h"
#include "AICondition.h"
#include "Logic/Object/Actor/Actor.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
AI::AI()
{
    // AI 트리 구성 예제:
    // 최상위 Selector가 아래의 순서로 판단:
    // 1. [Sequence] 공격 조건 → 공격 행동
    // 2. [Sequence] 대상 가시 조건 → 추격 행동
    // 3. 기본 이동 행동
    AINode* attackSequence = new AISequence(
        {
            new AICondition(
                []( Actor* actor )
                {
                    std::cout << "공격 범위 조건 검사: ";
                    // 실제 조건 로직 구현 가능 (예: 대상과의 거리가 충분히 가까운지)
                    return false; // 예제에서는 조건 불만족 처리
                } ),

            new AIAttackAction()
        } );

    AINode* chaseSequence = new AISequence(
        {
            new AICondition(
                []( Actor* actor )
                {
                    std::cout << "대상 시야 조건 검사: ";
                    // 실제 조건 로직 구현 가능 (예: 대상이 시야에 있는지)
                    return true; // 예제에서는 조건 만족 처리
                } ),

            new AIChaseAction()
        } );

    AINode* moveAction = new AIMoveAction();

    root = new AISelector( { attackSequence, chaseSequence, moveAction } );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소멸자
////////////////////////////////////////////////////////////////////////////////////////////////////
AI::~AI()
{
    if ( root )
        delete root;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 업데이트
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid AI::Update( Actor* actor )
{
	if ( root )
		root->Execute( actor );
}
