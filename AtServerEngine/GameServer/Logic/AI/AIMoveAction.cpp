////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif AIMoveAction class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "AIMoveAction.h"
#include "MapData/SceneManager.h"
#include "Room/Room.h"
#include "Room/PlayRoom.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 실행한다
////////////////////////////////////////////////////////////////////////////////////////////////////
AIMoveAction::AIMoveAction( const set< int >& movePath )
{
	m_movePath.clear();

	for ( int path : movePath )
		m_movePath.push_back( path );

	m_curNodeIndex = 0;
	m_sceneManager = nullptr;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 실행한다
////////////////////////////////////////////////////////////////////////////////////////////////////
AIStatus AIMoveAction::Execute( Actor* actor, Millisecond curTime )
{
	if ( !actor )
		return AIStatus::Failure;

	if ( !m_sceneManager && !_SetSceneManager( actor ) )
		return AIStatus::Failure;

	if( !_CheckMove( actor ) )
		return AIStatus::Failure;

	// 0.1s 마다 움직인다.
	AtInt64 deltaTime = 100;
	if ( curTime.count() - m_lastUpdateTime.count() < deltaTime )
		return AIStatus::Failure;

	auto destPos = m_sceneManager->GetWorldPosByNodeId( m_movePath[ m_curNodeIndex ] );
	if ( actor->IsSameNode( destPos.first, destPos.second ) )
	{
		// INFO_LOG( "SetNextNode" );
		m_curNodeIndex++;
		return AIStatus::Success;
	}
	else
	{
		float newX = 0.0f;
		float newZ = 0.0f;

		_ExporToNextPos( actor, deltaTime, destPos.first, destPos.second, newX, newZ );

		actor->posInfo->set_x( newX );
		actor->posInfo->set_z( newZ );

		m_lastUpdateTime = curTime;
		actor->SetIsMoveUpdate( true );

		return AIStatus::Success;
	}

	return AIStatus::Failure;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Actor의 씬매니저를 반환받는다.
////////////////////////////////////////////////////////////////////////////////////////////////////
bool AIMoveAction::_SetSceneManager( Actor* actor )
{
	auto room = actor->room.load().lock();
	if ( !room )
		return false;

	auto playRoom = std::dynamic_pointer_cast<PlayRoom>( room );
	if ( !playRoom )
		return false;

	m_sceneManager = playRoom->GetSceneManager();
	return m_sceneManager;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 이동이 가능한지 확인한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
bool AIMoveAction::_CheckMove( Actor* actor ) const
{
	// if ( actor && !actor->IsMobeable() )
	// 	return false;

	if ( m_movePath.size() <= m_curNodeIndex )
		return false;

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif Actor의 다음 좌표를 내보낸다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void AIMoveAction::_ExporToNextPos( Actor* actor, float deltaTime, float destX, float destZ, float& newX, float& newZ )
{
	float speed = 3.0f; // TODO : actor별로 속도 가져오기

	float curX = actor->posInfo->x();
	float curZ = actor->posInfo->z();

	float dx = destX - curX;
	float dz = destZ - curZ;

	// 목표까지의 거리
	float dist = std::sqrt( dx * dx + dz * dz );
	if ( dist < 0.0001f ) // 너무 가까우면 이동 불필요
	{
		newX = curX;
		newZ = curZ;
		return;
	}

	// 단위 벡터 (정규화)
	float nx = dx / dist;
	float nz = dz / dist;

	// 이동 거리 (speed * deltaTime) 
	float moveDist = speed * ( deltaTime / 1000.0f );

	// 목표보다 멀리 가지 않게 clamp
	if ( moveDist > dist )
		moveDist = dist;

	// 새로운 좌표
	newX = curX + nx * moveDist;
	newZ = curZ + nz * moveDist;
}
