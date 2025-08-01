set Server=GameServer

GenProcs.exe --path=../../%Server%/DB/TitleDB.xml --dbName=atserver_game

XCOPY /Y GenProcedures.h "../../%Server%/DB"
DEL /Q /F *.h

IF ERRORLEVEL 1 PAUSE