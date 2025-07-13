#pragma once
#include "DBConnection.h"
#include "DBModel.h"

/*--------------------
	DBSynchronizer
---------------------*/

class DBSynchronizer
{
	enum
	{
		PROCEDURE_MAX_LEN = 10000
	};

	enum UpdateStep : uint8
	{
		DropIndex,
		AlterColumn,
		AddColumn,
		CreateTable,
		DefaultConstraint,
		CreateIndex,
		DropColumn,
		DropTable,
		StoredProcecure,

		Max
	};

	enum ColumnFlag : uint8
	{
		Type = 1 << 0,
		Nullable = 1 << 1,
		Identity = 1 << 2,
		Default = 1 << 3,
		Length = 1 << 4,
	};

public:
	enum class EType : uint8
	{
		Title = 1,
		Game  = 2,
	};

public:
	DBSynchronizer( EType type, DBConnection& conn ) : _type( type ), _dbConn( conn ) {}
	~DBSynchronizer();

	bool		Synchronize(const WCHAR* path);

private:
	void		ParseXmlDB(const WCHAR* path);

	bool		GatherDBTables();
	bool		GatherTitleDBTables();

	bool		GatherDBIndexes();
	bool		GatherDBStoredProcedures();

	void		CompareDBModel();
	void		CompareTables(DBModel::TablePtr dbTable, DBModel::TablePtr xmlTable);
	void		CompareColumns(DBModel::TablePtr dbTable, DBModel::ColumnPtr dbColumn, DBModel::ColumnPtr xmlColumn);
	void		CompareStoredProcedures();

	void		ExecuteUpdateQueries();

private:
	EType                               _type;
	DBConnection&                       _dbConn;

	Vector<DBModel::TablePtr>			_xmlTables;
	Vector<DBModel::ProcedurePtr>		_xmlProcedures;
	Set<String>							_xmlRemovedTables;

	Vector<DBModel::TablePtr>			_dbTables;
	Vector<DBModel::ProcedurePtr>		_dbProcedures;

private:
	Set<String>							_dependentIndexes;
	Vector<String>						_updateQueries[UpdateStep::Max];
};