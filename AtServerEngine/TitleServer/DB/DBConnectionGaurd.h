#pragma once


class DBConnectionGaurd
{
public:
	DBConnectionGaurd()
	{
		_dBConnection = GDBConnectionPool->Pop();
	}

	~DBConnectionGaurd()
	{
		GDBConnectionPool->Push( _dBConnection );
	}

	DBConnection* operator->() const
	{
		return _dBConnection;
	}

	DBConnection& operator*() const
	{
		return *_dBConnection;
	}

private:
	DBConnection* _dBConnection = nullptr;
};