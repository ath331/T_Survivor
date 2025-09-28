////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AINode class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once


class Actor;


enum class AIStatus
{
	Success,
	Failure,
	Running
};


class AINode
{
public:
	/// 소멸자
	virtual ~AINode() {}

	/// 실행한다
	virtual AIStatus Execute( Actor* actor, Millisecond curTime ) = 0;

protected:
	/// 가장 최근에 업데이트된 시간
	Millisecond m_lastUpdateTime = (Millisecond)( 0 );
};
