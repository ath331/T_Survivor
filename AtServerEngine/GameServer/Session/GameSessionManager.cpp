#include "pch.h"
#include "GameSessionManager.h"
#include "GameSession.h"

GameSessionManager GSessionManager;

void GameSessionManager::Add(GameSessionRef session)
{
	WRITE_LOCK;
	_sessions.insert(session);
	m_sessionMap[ session->GetSessionId() ] = session;
}

void GameSessionManager::Remove(GameSessionRef session)
{
	WRITE_LOCK;
	_sessions.erase(session);

	auto iter = m_sessionMap.find( session->GetSessionId() );
	if ( iter != m_sessionMap.end() )
		m_sessionMap.erase( iter );
}

void GameSessionManager::Broadcast(SendBufferPtr sendBuffer)
{
	WRITE_LOCK;
	for (GameSessionRef session : _sessions)
	{
		session->Send(sendBuffer);
	}
}