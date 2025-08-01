set Server=TitleServer

GenProcs.exe --path=../../%Server%/DB/TitleDB.xml --dbName=atserver_title

XCOPY /Y GenProcedures.h "../../%Server%/DB"
DEL /Q /F *.h

IF ERRORLEVEL 1 PAUSE