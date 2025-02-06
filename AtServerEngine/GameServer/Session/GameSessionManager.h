#pragma once

class GameSession;

using GameSessionRef = shared_ptr<GameSession>;

class GameSessionManager
{
public:
	void Add(GameSessionRef session);
	void Remove(GameSessionRef session);
	void Broadcast(SendBufferPtr sendBuffer);

private:
	USE_LOCK;
	Set<GameSessionRef> _sessions;

	/// Session �ĺ��ں� Session ����
	Map< AtInt64, GameSessionRef > m_sessionMap;
};

extern GameSessionManager GSessionManager;
