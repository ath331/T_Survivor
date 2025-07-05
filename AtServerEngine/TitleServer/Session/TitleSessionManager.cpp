#include "pch.h"
#include "TitleSessionManager.h"
#include "TitleSession.h"

TitleSessionManager GSessionManager;

void TitleSessionManager::Add(TitleSessionRef session)
{
	WRITE_LOCK;
	_sessions.insert(session);
	m_sessionMap[ session->GetSessionId() ] = session;
}

void TitleSessionManager::Remove(TitleSessionRef session)
{
	WRITE_LOCK;
	_sessions.erase(session);

	auto iter = m_sessionMap.find( session->GetSessionId() );
	if ( iter != m_sessionMap.end() )
		m_sessionMap.erase( iter );
}

void TitleSessionManager::Broadcast(SendBufferPtr sendBuffer)
{
	WRITE_LOCK;
	for (TitleSessionRef session : _sessions)
	{
		session->Send(sendBuffer);
	}
}