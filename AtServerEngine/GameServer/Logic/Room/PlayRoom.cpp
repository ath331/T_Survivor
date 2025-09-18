////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif PlayRoom File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "PlayRoom.h"
#include "Packet/Protocol.pb.h"
#include "Logic/Utils/Time/AtTime.h"
#include "MapData/SceneManager.h"
#include "Session/GameSession.h"
#include "Logic/Spawn/MonsterSpawnManager.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
PlayRoom::PlayRoom()
{
	// 대기 시간 보정
	m_startTime = AtTime::GetCurMillisecond() + (Millisecond)( 7000 );

	// TODO : 임시 데이터 그룹을 사용중
	AtInt32 spawnGroupId = 100;
	m_monsterSpawnManager = new MonsterSpawnManager( this, spawnGroupId );

	m_sceneManager = new SceneManager( Environment::Get( "ExePath" ) + "/../../../AtClientFramework/Assets/Resources/SceneJson/GameMap.json" ); // TODO : 이쁘게 수정해야할덧
	m_sceneManager->DrawSceneMap();
	movePath = m_sceneManager->FindPath( 270, 1945 ); // npc 움직임 보려는 임시 코드
	m_sceneManager->DrawGraph( true, true );
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 소멸자
////////////////////////////////////////////////////////////////////////////////////////////////////
PlayRoom::~PlayRoom()
{
	if ( m_monsterSpawnManager )
		delete m_monsterSpawnManager;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 룸을 업데이트한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
AtVoid PlayRoom::UpdateTick( Millisecond curTime )
{
	if ( m_monsterSpawnManager )
		m_monsterSpawnManager->Update();

	if ( m_startTime.count() < curTime.count() )
	{
		if ( m_isPrintPath )
		{
			for ( auto rIter = movePath.rbegin(); rIter != movePath.rend(); rIter++ )
			{
				int nextNode = *rIter;

				auto worldPos = m_sceneManager->GetWorldPosByNodeId( nextNode );
				cout << worldPos.first << ", " << worldPos.second << endl;

				S_Move move;
				move.set_result( EResultCode::RESULT_CODE_SUCCESS );
				move.mutable_objectinfo()->set_id( 1000 );
				move.mutable_objectinfo()->mutable_pos_info()->set_id( 1000 );
				move.mutable_objectinfo()->mutable_pos_info()->set_x( worldPos.first );
				move.mutable_objectinfo()->mutable_pos_info()->set_z( worldPos.second );

				Broadcast( move );
				std::this_thread::sleep_for( 1s );
			}

			m_isPrintPath = false;
		}
	}

	Room::UpdateTick( curTime );
}
