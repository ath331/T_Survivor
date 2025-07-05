////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AtLog File
////////////////////////////////////////////////////////////////////////////////////////////////////


// TODO : core의 콘솔로그 이용하도록 변경하기


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
		None  = 0, // 기본값
		Red   = 1, // 빨간색
		Green = 2, // 초록색
	};
			
public:
	// 생성자
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

	// 소멸자
	~AtStringColor()
	{
		SetConsoleTextAttribute( m_handle, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE );
	}

private:
	// 핸들 값
	HANDLE m_handle;

	// 현재 컬러
	EColor m_color = EColor::None;
};


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief AtLog Class
////////////////////////////////////////////////////////////////////////////////////////////////////
class AtLog
{
public:
	/// msg를 출력한다.
	static AtVoid PrintMsg(
		const char*                 file,
		      AtInt32               line,
		      AtString              msg,
		      AtStringColor::EColor color = AtStringColor::EColor::None );

	/// msg를 출력한다.
	static AtVoid PrintNoFileMsg( AtString msg, AtStringColor::EColor color = AtStringColor::EColor::None );

	/// PacketData 출력을 시도한다.
	static AtVoid TryPrintPacketData( google::protobuf::Message& pkt );

	/// PacketData를 출력한다.
	static AtVoid PrintPacketData( google::protobuf::Message& pkt );

private:
	/// 파일 경로에서 파일 이름만 추출하는 함수
	static const char* extractFileName( const char* filePath );

private:
	/// 패킷 출력을 하는 타입인지 확인한다.
	static AtBool _CheckPrintPakcetType( const AtString& packetType );
};