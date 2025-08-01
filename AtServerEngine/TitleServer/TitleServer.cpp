////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief TitleServer.cpp
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "ThreadManager.h"
#include "Service.h"
#include "Session.h"
#include "Session/TitleSession.h"
#include "Session/TitleSessionManager.h"
#include "BufferWriter.h"
#include <tchar.h>
#include <format>
#include "Job.h"
#include "DBConnectionPool.h"
#include "DBBind.h"
#include "XmlParser.h"
#include "DBSynchronizer.h"
#include "DB/GenProcedures.h"
#include "Packet/Handler/ClientPacketHandler.h"
#include "Packet/Protocol.pb.h"


#include "Logic/Utils/Time/AtTime.h"


/// 프로세스 틱 이넘
enum
{
	WORKER_TICK = 64
};


////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief 쓰레드가 동작하는 함수
////////////////////////////////////////////////////////////////////////////////////////////////////
void DoWorkerJob( ServerServicePtr& service )
{
	while ( true )
	{
		LEndTickCount = ::GetTickCount64() + WORKER_TICK;

		// 네트워크 입출력 처리 -> 인게임 로직까지 (패킷 핸들러에 의해)
		service->GetIocpCore()->Dispatch( 10 );

		// 예약된 일감 처리
		ThreadManager::DistributeReservedJobs();

		// 글로벌 큐
		ThreadManager::DoGlobalQueueWork();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @brief ServerMain 함수
////////////////////////////////////////////////////////////////////////////////////////////////////
AtInt32 main()
{
#ifdef _WIN32
	SetConsoleOutputCP( CP_UTF8 );
#endif

	if ( !Environment::Load( "../Binary/Release/TitleServer.ini" ) )
	{
		if ( !Environment::Load( "TitleServer.ini" ) )
		{
			WARNNING_LOG( "Failed to load config.ini" );
			return -1;
		}
	}

	// SqlServer
	//ASSERT_CRASH( GDBConnectionPool->Connect( 1, L"Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\\ProjectModels;Database=AtServer;Trusted_Connection=Yes;" ) );

	// MySql
	if ( StringUtils::GetBool( Environment::Get( "DB_CONNECT" ) ) )
	{
		AtString connect = std::format( "Driver={{MySQL ODBC 8.2 UNICODE Driver}};Server={};Port={};Database={};User={};Password={};",
										Environment::Get( "DB_IP" ),
										Environment::Get( "DB_PORT" ),
										Environment::Get( "DB_NAME" ),
										Environment::Get( "DB_USER" ),
										Environment::Get( "DB_PW" ) );
	
		ASSERT_CRASH( GDBConnectionPool->Connect( 1, StringUtils::ConvertToWString( connect ).c_str() ) );
	
		DBConnectionGaurd dbConn;
		DBSynchronizer dbSync( DBSynchronizer::EType::Title, *dbConn );
		dbSync.Synchronize( StringUtils::ConvertToWString( Environment::Get( "DB_ASSET_PATH" ) ).c_str() );
	}

	ClientPacketHandler::Init();

	AtString ip = Environment::Get( "IP" );
	AtString port = Environment::Get( "PORT" );

	ServerServicePtr service = MakeShared< ServerService >(
		NetAddress( StringUtils::ConvertToWString( ip ), StringUtils::GetAtInt64( port ) ),
		MakeShared< IocpCore >(),
		MakeShared< TitleSession >, // TODO : SessionManager 등
		100 );

	{
		DBConnectionGaurd dbConn;
		
		SP::GetServerList getServerList( *dbConn );
		
		WCHAR name[ 100 ];
		WCHAR ip[ 100 ];
		int port = 0;
		
		getServerList.Out_Name( OUT name );
		getServerList.Out_Ip  ( OUT ip );
		getServerList.Out_Port( OUT port );
		getServerList.Execute();

		while ( getServerList.Fetch() )
		{
			GConsoleLogger->WriteStdOut( Color::BLUE, L"Name[%s] ip[%s] port[%d]\n", name, ip, port );
		}
	}

	if ( !service->Start() )
	{
		WARNNING_LOG( AtString( "ERROR :" + std::to_string( WSAGetLastError() ) ) );
		return -1;
	}

	INFO_LOG_GREEN( ip + ":" + port + " Server Start." );

	int32 threadCount = 6;
	for ( int32 i = 0; i < threadCount - 1; i++ )
	{
		GThreadManager->Launch(
			[ &service ]()
			{
				DoWorkerJob( service );
			} );
	}

	Millisecond curTime = AtTime::GetCurMillisecond();

	GThreadManager->Join();

	return 0;
}