#pragma once
#include "Types.h"
#include <windows.h>
#include "DBBind.h"

namespace SP
{
	
    class GetServerList : public DBBind<0,3>
    {
    public:
    	GetServerList(DBConnection& conn) : DBBind(conn, L"{CALL atserver_title.spGetServerList}") { }
    	template<int32 N> void Out_Name(OUT WCHAR(&v)[N]) { BindCol(0, v); };
    	template<int32 N> void Out_Ip(OUT WCHAR(&v)[N]) { BindCol(1, v); };
    	void Out_Port(OUT int32& v) { BindCol(2, v); };

    private:
    };


     
};