using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 에디터에서만 사용한다는 뜻으로, #endif 까지의 구문들은 빌드에 포함되지 않는다 */
#if UNITY_EDITOR
using UnityEditor;

public class CustomEditorWindow : EditorWindow
{
    private bool   isToolVarOpen = false;
    private string xlsPath       = "../AtServerEngine/Data/"; // 프로젝트 경로 기준. ( AtClientFrameWork/ )
    private string destJsonPath  = "Assets/Data/";            // 프로젝트 경로 기준. ( AtClientFrameWork/ )

    [MenuItem( "Custom/EditorWindow" )] //해당 버튼을 누르면 Init 함수가 실행
    private static void Init()
    {
        CustomEditorWindow editorWindow = (CustomEditorWindow)( GetWindow( typeof( CustomEditorWindow ) ) );
        editorWindow.Show();
    }

    private void OnGUI()
    {
        isToolVarOpen = EditorGUILayout.Foldout( isToolVarOpen, "DataTool", true );
        if ( isToolVarOpen  )
        {
            EditorGUI.indentLevel++;

            xlsPath = EditorGUILayout.TextField( "SourcePath", xlsPath );
            destJsonPath = EditorGUILayout.TextField( "DestPath", destJsonPath );

            if ( GUILayout.Button( "ConvertToJson" ) )
            {
                string projectPath = System.IO.Directory.GetParent( Application.dataPath ).ToString();
                _ConvertToJson( projectPath + "/" + xlsPath, projectPath + "/" + destJsonPath );
            }

            EditorGUI.indentLevel--;
        }

        string a = "레이아웃 테스트용1";
        EditorGUILayout.TextField( "Test", a );
        string b = "레이아웃 테스트용2";
        EditorGUILayout.TextField( "Test", b );
    }

    private void _ConvertToJson( string sourcePath, string destPath )
    {

    }
}
#endif
