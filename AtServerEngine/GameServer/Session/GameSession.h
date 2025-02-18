#pragma once
#include "Session.h"


class GameSession : public PacketSession
{
private:
	/// Session 식별자
	AtInt64 m_sessionId = 0;

public:
	~GameSession()
	{
		cout << "~GameSession" << endl;
	}

	/// Session 식별자를 세팅한다.
	AtVoid SetSessionId( AtInt64 sessionId ) { m_sessionId = sessionId; }

	/// Session 식별자를 반환한다.
	AtInt64 GetSessionId() { return m_sessionId; }

public:
	virtual void OnConnected() override;
	virtual void OnDisconnected() override;
	virtual void OnRecvPacket( BYTE* buffer, int32 len ) override;
	virtual void OnSend( int32 len ) override;

public:
	/// Send 한다.
	AtVoid Send( google::protobuf::Message& pkt );

	/// Send 한다.
	AtVoid Send( SendBufferPtr sendBuffer );

public:
	atomic< PlayerPtr > player;
};
