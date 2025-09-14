using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.AI;

public class MapExporter : MonoBehaviour
{
    [MenuItem( "Custom/ExportMapData(CurScene)" )]
    public static void ExportSceneMap()
    {
        var sb = new StringBuilder();
        sb.AppendLine( "{" );

        // ==== 1) Objects ====
        //sb.AppendLine( "  \"objects\": [" );
        //bool firstObj = true;
        //
        //Tilemap refTilemap = Object.FindObjectOfType<Tilemap>();
        //var objs = Object.FindObjectsOfType<GameObject>();
        //foreach ( var obj in objs )
        //{
        //    // Unity 시스템 오브젝트 제외
        //    if ( obj.hideFlags != HideFlags.None )
        //        continue;
        //    if ( obj.GetComponent<Camera>() != null )
        //        continue;
        //    if ( obj.GetComponent<Light>() != null )
        //        continue;
        //
        //    if ( !firstObj )
        //        sb.AppendLine( "," );
        //    firstObj = false;
        //
        //    Vector3 worldPos = obj.transform.position;
        //    Vector3Int cellPos = refTilemap != null
        //        ? refTilemap.WorldToCell( worldPos )
        //        : Vector3Int.zero;
        //
        //    sb.Append( "    {" );
        //    sb.Append( $"\"name\":\"{obj.name}\"," );
        //    sb.Append( $"\"worldX\":{worldPos.x},\"worldY\":{worldPos.y},\"worldZ\":{worldPos.z}," );
        //    sb.Append( $"\"cellX\":{cellPos.x},\"cellY\":{cellPos.y},\"cellZ\":{cellPos.z}" );
        //    sb.Append( "}" );
        //}
        //sb.AppendLine();
        //sb.AppendLine( "  ]," );

        // ==== 2) NavMesh ====
        NavMeshTriangulation tri = NavMesh.CalculateTriangulation();
        sb.AppendLine( "  \"navmesh\": {" );

        // Vertices
        sb.AppendLine( "    \"vertices\": [" );
        for ( int i = 0 ; i < tri.vertices.Length ; i++ )
        {
            var v = tri.vertices[ i ];
            sb.Append( $"      [{v.x},{v.y},{v.z}]" );
            if ( i < tri.vertices.Length - 1 )
                sb.Append( "," );
            sb.AppendLine();
        }
        sb.AppendLine( "    ]," );

        // Triangles
        sb.AppendLine( "    \"triangles\": [" );
        for ( int i = 0 ; i < tri.indices.Length ; i += 3 )
        {
            sb.Append( $"      [{tri.indices[ i ]},{tri.indices[ i + 1 ]},{tri.indices[ i + 2 ]}]" );
            if ( i < tri.indices.Length - 3 )
                sb.Append( "," );
            sb.AppendLine();
        }
        sb.AppendLine( "    ]," );

        // Areas
        sb.AppendLine( "    \"areas\": [" );
        for ( int i = 0 ; i < tri.areas.Length ; i++ )
        {
            sb.Append( $"      {tri.areas[ i ]}" );
            if ( i < tri.areas.Length - 1 )
                sb.Append( "," );
            sb.AppendLine();
        }
        sb.AppendLine( "    ]" );

        sb.AppendLine( "  }" ); // navmesh
        sb.AppendLine( "}" );

        // ==== Save File ====
        var sceneName = SceneManager.GetActiveScene().name;
        string dirPath = Application.dataPath + "/Resources/SceneJson/";
        if ( !Directory.Exists( dirPath ) )
            Directory.CreateDirectory( dirPath );

        string path = dirPath + $"{sceneName}Map.json";
        File.WriteAllText( path, sb.ToString(), Encoding.UTF8 );

        Debug.Log( $"씬 맵 데이터 Export 완료: {path}" );
        AssetDatabase.Refresh();
    }
}
