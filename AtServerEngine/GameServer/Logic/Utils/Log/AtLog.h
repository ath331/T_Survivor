////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AtLog File
////////////////////////////////////////////////////////////////////////////////////////////////////


// TODO : core�� �ַܼα� �̿��ϵ��� �����ϱ�


#pragma once
#include <google/protobuf/message.h>
#include <google/protobuf/any.pb.h>


#define INFO_LOG( msg )    AtLog::PrintMsg( __FILE__, __LINE__, msg );
#define INFO_LOG_GREEN( msg )    AtLog::PrintMsg( __FILE__, __LINE__, msg, AtStringColor::EColor::Green );
#define WARNNING_LOG( msg ) AtLog::PrintMsg( __FILE__, __LINE__, msg, AtStringColor::EColor::Red );

#define INFO_LOG_NO_LINE( msg ) AtLog::PrintNoFileMsg( msg );
#define PKT_LOG( pkt ) AtLog::TryPrintPacketData( pkt );


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AtStringColor Class
////////////////////////////////////////////////////////////////////////////////////////////////////
class AtStringColor
{
public:
	enum class EColor
	{
		None  = 0, // �⺻��
		Red   = 1, // ������
		Green = 2, // �ʷϻ�
	};
			
public:
	// ������
	AtStringColor( EColor color )
		:
		m_color( color )
	{
		if ( m_color == EColor::None )
			return;

		m_handle = GetStdHandle( STD_OUTPUT_HANDLE );

		if ( m_color == EColor::Red )
			SetConsoleTextAttribute( m_handle, FOREGROUND_RED );
		else if ( m_color == EColor::Green )
			SetConsoleTextAttribute( m_handle, FOREGROUND_GREEN );
	}

	// �Ҹ���
	~AtStringColor()
	{
		SetConsoleTextAttribute( m_handle, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE );
	}

private:
	// �ڵ� ��
	HANDLE m_handle;

	// ���� �÷�
	EColor m_color = EColor::None;
};


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AtLog Class
////////////////////////////////////////////////////////////////////////////////////////////////////
class AtLog
{
public:
	/// msg�� ����Ѵ�.
	static AtVoid PrintMsg(
		const char*                 file,
		      AtInt32               line,
		      AtString              msg,
		      AtStringColor::EColor color = AtStringColor::EColor::None );

	/// msg�� ����Ѵ�.
	static AtVoid PrintNoFileMsg( AtString msg, AtStringColor::EColor color = AtStringColor::EColor::None );

	/// PacketData ����� �õ��Ѵ�.
	static AtVoid TryPrintPacketData( google::protobuf::Message& pkt );

	/// PacketData�� ����Ѵ�.
	static AtVoid PrintPacketData( google::protobuf::Message& pkt );

private:
	/// ���� ��ο��� ���� �̸��� �����ϴ� �Լ�
	static const char* extractFileName( const char* filePath );

private:
	/// ��Ŷ ����� �ϴ� Ÿ������ Ȯ���Ѵ�.
	static AtBool _CheckPrintPakcetType( const AtString& packetType );
};