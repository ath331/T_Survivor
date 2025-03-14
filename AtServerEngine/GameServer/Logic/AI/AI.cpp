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
// @breif ������
////////////////////////////////////////////////////////////////////////////////////////////////////
AI::AI()
{
    // AI Ʈ�� ���� ����:
    // �ֻ��� Selector�� �Ʒ��� ������ �Ǵ�:
    // 1. [Sequence] ���� ���� �� ���� �ൿ
    // 2. [Sequence] ��� ���� ���� �� �߰� �ൿ
    // 3. �⺻ �̵� �ൿ
    AINode* attackSequence = new AISequence(
        {
            new AICondition(
                []( Actor* actor )
                {
                    std::cout << "���� ���� ���� �˻�: ";
                    // ���� ���� ���� ���� ���� (��: ������ �Ÿ��� ����� �������)
                    return false; // ���������� ���� �Ҹ��� ó��
                } ),

            new AIAttackAction()
        } );

    AINode* chaseSequence = new AISequence(
        {
            new AICondition(
                []( Actor* actor )
                {
                    std::cout << "��� �þ� ���� �˻�: ";
                    // ���� ���� ���� ���� ���� (��: ����� �þ߿� �ִ���)
                    return true; // ���������� ���� ���� ó��
                } ),

            new AIChaseAction()
        } );

    AINode* moveAction = new AIMoveAction();

    root = new AISelector( { attackSequence, chaseSequence, moveAction } );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif �Ҹ���
////////////////////////////////////////////////////////////////////////////////////////////////////
AI::~AI()
{
    if ( root )
        delete root;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif ������Ʈ
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid AI::Update( Actor* actor )
{
	if ( root )
		root->Execute( actor );
}
