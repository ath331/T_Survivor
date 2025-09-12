////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif CT_ServerListReadHandler class
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "CT_ServerListReadHandler.h"


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif HandlerRun
////////////////////////////////////////////////////////////////////////////////////////////////////
AtBool CT_ServerListReadHandler::Handle( PacketSessionPtr& session, Protocol::CT_ServerListRead& pkt )
{
	ST_ServerListRead result;

	{
		DBConnectionGaurd dbConn;

		SP::GetServerList getServerList( *dbConn );

		WCHAR name[ 100 ];
		WCHAR ip[ 100 ];
		int port = 0;

		getServerList.Out_Name( OUT name );
		getServerList.Out_Ip( OUT ip );
		getServerList.Out_Port( OUT port );
		getServerList.Execute();

		while ( getServerList.Fetch() )
		{
			GConsoleLogger->WriteStdOut( Color::BLUE, L"Name[%s] ip[%s] port[%d]\n", name, ip, port );

			ServerInfo* serverInfo = result.add_serverinfolist();
			serverInfo->set_name( StringUtils::ConvertToString( name ) );
			serverInfo->set_ip  ( StringUtils::ConvertToString( ip )   );
			serverInfo->set_port( port );
		}
	}

	result.set_result( EResultCode::RESULT_CODE_SUCCESS );


	SendBufferPtr sendBuffer = ClientPacketHandler::MakeSendBuffer( result );

	session->Send( sendBuffer );

	return true;
}