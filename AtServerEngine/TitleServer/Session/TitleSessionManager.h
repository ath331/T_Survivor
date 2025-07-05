#pragma once

class TitleSession;

using TitleSessionRef = shared_ptr<TitleSession>;

class TitleSessionManager
{
public:
	void Add(TitleSessionRef session);
	void Remove(TitleSessionRef session);
	void Broadcast(SendBufferPtr sendBuffer);

private:
	USE_LOCK;
	Set<TitleSessionRef> _sessions;

	/// Session 식별자별 Session 정보
	Map< AtInt64, TitleSessionRef > m_sessionMap;
};

extern TitleSessionManager GSessionManager;
