////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AIMoveAction class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include "AINode.h"


class SceneManager;


class AIMoveAction
	:
	public AINode
{
private:
	/// 이동 경로
	vector<int> m_movePath;

	/// 현재 노드의 인덱스
	int m_curNodeIndex;

	/// 씬 매니저
	SceneManager* m_sceneManager;

public:
	/// 생성자
	AIMoveAction( const set< int >& movePath );

	/// 실행한다
	virtual AIStatus Execute( Actor* actor, Millisecond curTime ) override;

private:
	/// Actor의 씬매니저를 반환받는다.
	bool _SetSceneManager( Actor* actor );

	/// 이동이 가능한지 확인한다.
	bool _CheckMove( Actor* actor ) const;

	/// Actor의 다음 좌표를 내보낸다.
	void _ExporToNextPos( Actor* actor, float deltaTime, float destX, float destZ, float& newX, float& newZ );
};
