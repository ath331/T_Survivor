using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using ExcelDataReader;
using System.IO;
using System;
using UnityEngine.Windows;
using File = System.IO.File;


/* �����Ϳ����� ����Ѵٴ� ������, #endif ������ �������� ���忡 ���Ե��� �ʴ´� */
#if UNITY_EDITOR
using UnityEditor;

public class CustomEditorWindow : EditorWindow
{
    private bool isToolVarOpen = true;
    private string xlsPath = "../AtServerEngine/Data/"; // ������Ʈ ��� ����. ( AtClientFrameWork/ )
    private string destJsonPath = "Assets/Data/";            // ������Ʈ ��� ����. ( AtClientFrameWork/ )

    [MenuItem( "Custom/EditorWindow" )] //�ش� ��ư�� ������ Init �Լ��� ����
    private static void Init()
    {
        CustomEditorWindow editorWindow = (CustomEditorWindow)( GetWindow( typeof( CustomEditorWindow ) ) );
        editorWindow.Show();
    }

    private void OnGUI()
    {
        isToolVarOpen = EditorGUILayout.Foldout( isToolVarOpen, "DataTool", true );
        if ( isToolVarOpen )
        {
            EditorGUI.indentLevel++;

            xlsPath = EditorGUILayout.TextField( "SourcePath", xlsPath );
            destJsonPath = EditorGUILayout.TextField( "DestPath", destJsonPath );

            if ( GUILayout.Button( "ConvertToJson" ) )
            {
                string projectPath = System.IO.Directory.GetParent( Application.dataPath ).ToString();

                List<string> excelFiles = _ExportToAllExcel( projectPath + "/" + xlsPath );
                if ( 0 < excelFiles.Count )
                {
                    foreach ( var file in excelFiles )
                    {
                        string excelName = Path.GetFileNameWithoutExtension( file );
                        string relativePath = _GetRelativePath( projectPath + "/" + xlsPath, file );
                        string middlePath = _GetMiddlePath( relativePath );

                        _ConvertToJson( projectPath + "/" + xlsPath + middlePath, projectPath + "/" + destJsonPath, excelName );
                    }
                }
                else
                {
                    Debug.Log( "No Find Excel." );
                }

                return;
            }

            EditorGUI.indentLevel--;
        }
    }

    private List<string> _ExportToAllExcel( string sourcePath )
    {
        List<string> excelFiles = new List<string>();

        try
        {
            string[] files = System.IO.Directory.GetFiles( sourcePath, "*.xlsx", SearchOption.AllDirectories );

            // ����Ʈ�� �߰�
            excelFiles.AddRange( files );
        }
        catch ( Exception e )
        {
            Debug.LogError( $"���� �˻� �� ���� �߻�: {e.Message}" );
        }

        return excelFiles;
    }

    private string _GetRelativePath( string basePath, string fullPath )
    {
        // ���� ��θ� ��� ��η� ��ȯ
        string relativePath = Path.GetRelativePath( basePath, fullPath );
        return relativePath.Replace( "\\", "/" ); // ��� ������ ����
    }

    private string _GetMiddlePath( string relativePath )
    {
        // ���� �̸� ����
        string fileName = Path.GetFileName( relativePath );

        // ���� �̸��� ������ �߰� ��θ� ��������
        string directoryPath = Path.GetDirectoryName( relativePath )?.Replace( "\\", "/" ) ?? "";

        // ���� �̸��� ���� �̸��� ������ �߰� ��� ��ȯ, �׷��� ������ �� ���ڿ� ��ȯ
        return string.IsNullOrEmpty( directoryPath ) ? "" : directoryPath + "/";
    }

    private void _ConvertToJson( string sourcePath, string destPath, string excelName )
    {
        string jsonToIndex = ""; // �ĺ��ڱ��� ���Ե� json
        short depth = 8;

        string filePath = sourcePath + excelName + ".xlsx";

        try
        {
            using ( var stream = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
            {
                using ( var reader = ExcelReaderFactory.CreateReader( stream ) )
                {
                    var result = reader.AsDataSet();

                    for ( int sheetCount = 0 ; sheetCount < result.Tables.Count ; sheetCount++ )
                    {
                        if ( jsonToIndex.Length == 0 )
                        {
                            JsonMaker.InsertBeginBrace( ref jsonToIndex );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.InsertSpace( ref jsonToIndex, 4 );
                            JsonMaker.InsertKey( ref jsonToIndex, "IndexKeyType" );
                            JsonMaker.InsertValue( ref jsonToIndex, result.Tables[ sheetCount ].Rows[ 3 ][ 0 ].ToString() );
                            JsonMaker.InsertRest( ref jsonToIndex );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                        }

                        for ( int row = 0 ; row < result.Tables[ sheetCount ].Rows.Count ; row++ )
                        {
                            //  0, 1, 2, 3 row�� ������ �����̹Ƿ� 4���� üũ
                            if ( row < 4 )
                                continue;

                            // Disable == 1�� row�� ���� ( �÷��� 1�� �׻� Disable )
                            if ( result.Tables[ sheetCount ].Rows[ row ][ 1 ].ToString() == "1" )
                                continue;

                            string json = ""; // �ĺ����� value json

                            for ( int column = 0 ; column < result.Tables[ sheetCount ].Columns.Count ; column++ )
                            {
                                if ( result.Tables[ sheetCount ].Rows[ 0 ][ column ].ToString() == "Disable" )
                                    continue;

                                // ���������� ����ϴ� �÷��̸� ����
                                if ( result.Tables[ sheetCount ].Rows[ 2 ][ column ].ToString() == "S" ||
                                     result.Tables[ sheetCount ].Rows[ 2 ][ column ].ToString() == "*S" ||
                                     result.Tables[ sheetCount ].Rows[ 2 ][ column ].ToString() == "#S" )
                                    continue;

                                if ( 0 < json.Length )
                                {
                                    JsonMaker.InsertRest( ref json );
                                    JsonMaker.InsertNewLine( ref json );
                                }

                                JsonMaker.InsertSpace( ref json, depth );
                                JsonMaker.InsertKey( ref json, result.Tables[ sheetCount ].Rows[ 0 ][ column ].ToString() );
                                JsonMaker.InsertNewLine( ref json );
                                JsonMaker.InsertSpace( ref json, depth );
                                JsonMaker.InsertBeginBrace( ref json );
                                JsonMaker.InsertNewLine( ref json );
                                depth += 4;

                                {
                                    JsonMaker.InsertSpace( ref json, depth );
                                    JsonMaker.InsertKey( ref json, "Type" );
                                    JsonMaker.InsertValue( ref json, result.Tables[ sheetCount ].Rows[ 3 ][ column ].ToString() );
                                    JsonMaker.InsertRest( ref json );
                                    JsonMaker.InsertNewLine( ref json );

                                    JsonMaker.InsertSpace( ref json, depth );
                                    JsonMaker.InsertKey( ref json, "Value" );
                                    JsonMaker.InsertValue( ref json, result.Tables[ sheetCount ].Rows[ row ][ column ].ToString() );
                                    JsonMaker.InsertRest( ref json );
                                    JsonMaker.InsertNewLine( ref json );

                                    JsonMaker.InsertSpace( ref json, depth );
                                    JsonMaker.InsertKey( ref json, "Desc" );
                                    JsonMaker.InsertValue( ref json, result.Tables[ sheetCount ].Rows[ 1 ][ column ].ToString() );
                                    JsonMaker.InsertNewLine( ref json );
                                }

                                depth -= 4;
                                JsonMaker.InsertSpace( ref json, depth );
                                JsonMaker.InsertEndBrace( ref json );
                            }

                            JsonMaker.InsertSpace( ref jsonToIndex, 4 );
                            JsonMaker.InsertKey( ref jsonToIndex, result.Tables[ sheetCount ].Rows[ row ][ 0 ].ToString() );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.InsertSpace( ref jsonToIndex, 4 );
                            JsonMaker.InsertBeginBrace( ref jsonToIndex );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.ConnectStr( ref jsonToIndex, json );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.InsertSpace( ref jsonToIndex, 4 );
                            JsonMaker.InsertEndBrace( ref jsonToIndex );

                            // ������ row�� �ƴ϶��
                            if ( row != result.Tables[ sheetCount ].Rows.Count - 1 )
                            {
                                JsonMaker.InsertRest( ref jsonToIndex );
                                JsonMaker.InsertNewLine( ref jsonToIndex );
                                JsonMaker.InsertNewLine( ref jsonToIndex );
                            }
                        }

                        if ( jsonToIndex.Length != 0 )
                        {
                            JsonMaker.InsertSpace( ref jsonToIndex, 4 );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                            JsonMaker.InsertEndBrace( ref jsonToIndex );
                            JsonMaker.InsertNewLine( ref jsonToIndex );
                        }
                    }
                }
            }

            string path = destPath + excelName + "Json.txt";

            if ( File.Exists( path ) )
                File.WriteAllText( path, "" );

            File.WriteAllText( path, jsonToIndex );
        }
        catch ( Exception e )
        {
        }
    }
}
#endif
