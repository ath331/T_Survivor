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

	/// Session 식별자별 Session 정보
	Map< AtInt64, GameSessionRef > m_sessionMap;
};

extern GameSessionManager GSessionManager;
