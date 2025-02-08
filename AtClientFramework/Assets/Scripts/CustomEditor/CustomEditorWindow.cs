using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* �����Ϳ����� ����Ѵٴ� ������, #endif ������ �������� ���忡 ���Ե��� �ʴ´� */
#if UNITY_EDITOR
using UnityEditor;

public class CustomEditorWindow : EditorWindow
{
    private bool   isToolVarOpen = false;
    private string xlsPath       = "../AtServerEngine/Data/"; // ������Ʈ ��� ����. ( AtClientFrameWork/ )
    private string destJsonPath  = "Assets/Data/";            // ������Ʈ ��� ����. ( AtClientFrameWork/ )

    [MenuItem( "Custom/EditorWindow" )] //�ش� ��ư�� ������ Init �Լ��� ����
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

        string a = "���̾ƿ� �׽�Ʈ��1";
        EditorGUILayout.TextField( "Test", a );
        string b = "���̾ƿ� �׽�Ʈ��2";
        EditorGUILayout.TextField( "Test", b );
    }

    private void _ConvertToJson( string sourcePath, string destPath )
    {

    }
}
#endif
