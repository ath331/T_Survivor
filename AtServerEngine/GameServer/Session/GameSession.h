#pragma once
#include "Session.h"


class GameSession : public PacketSession
{
private:
	/// Session �ĺ���
	AtInt64 m_sessionId = 0;

public:
	~GameSession()
	{
		cout << "~GameSession" << endl;
	}

	/// Session �ĺ��ڸ� �����Ѵ�.
	AtVoid SetSessionId( AtInt64 sessionId ) { m_sessionId = sessionId; }

	/// Session �ĺ��ڸ� ��ȯ�Ѵ�.
	AtInt64 GetSessionId() { return m_sessionId; }

public:
	virtual void OnConnected() override;
	virtual void OnDisconnected() override;
	virtual void OnRecvPacket( BYTE* buffer, int32 len ) override;
	virtual void OnSend( int32 len ) override;

public:
	/// Send �Ѵ�.
	AtVoid Send( google::protobuf::Message& pkt );

	/// Send �Ѵ�.
	AtVoid Send( SendBufferPtr sendBuffer );

public:
	atomic< PlayerPtr > player;
};
