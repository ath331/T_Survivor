using System;
using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] Grid grid;

    [SerializeField] Blocks[] tilePrefabs;

    public string seed;
    public bool useRandomSeed;

    [SerializeField] 
    private Vector2Int size;

    [SerializeField, Range( 0, 100 )]
    private int normalHeightPercent;

    [SerializeField, Range( 0, 100 )]
    private int normalBiomePercent;

    [SerializeField] private int smoothingFactor;

    public int[,] MapHeights;
    public int[,] MapBiomes;

    void Start()
    {
        GenerateMap();
        DrawMapTiles();
    }

    void GenerateMap()
    {
        MapHeights = new int[ size.x, size.y ];
        MapBiomes = new int[ size.x, size.y ];
        RandomFillMap();

        for ( int i = 0 ; i < smoothingFactor ; i++ )
        {
            SmoothMap( MapHeights );
            SmoothMap( MapBiomes );
        }
    }

    void RandomFillMap()
    {
        if ( useRandomSeed )
            seed = DateTime.Now.ToBinary().ToString();

        System.Random psuedoRandom = new System.Random( seed.GetHashCode() );

        for ( int x = 0 ; x < size.x ; x++ )
            for ( int y = 0 ; y < size.y ; y++ )
            {
                if ( x == 0 || x == size.x - 1 || y == 0 || y == size.y - 1 )
                    MapHeights[ x, y ] = 1;
                else
                    MapHeights[ x, y ] = psuedoRandom.Next( 0, 100 ) < normalHeightPercent ? 0 : 1;

                MapBiomes[ x, y ] = psuedoRandom.Next( 0, 100 ) < normalBiomePercent ? 0 : 1;
            }
    }

    void SmoothMap( int[,] map )
    {
        for ( int x = 0 ; x < size.x ; x++ )
            for ( int y = 0 ; y < size.y ; y++ )
            {
                int neighbourDifferentTiles = GetSurroundingTiles( map, x, y );

                if ( neighbourDifferentTiles > 4 )
                    map[ x, y ] = 1;
                else if ( neighbourDifferentTiles < 4 )
                    map[ x, y ] = 0;
            }
    }

    int GetSurroundingTiles( int[,] map, int gridX, int gridY )
    {
        int wallCount = 0;
        for ( int neighbourX = gridX - 1 ; neighbourX <= gridX + 1 ; neighbourX++ )
            for ( int neighbourY = gridY - 1 ; neighbourY <= gridY + 1 ; neighbourY++ )
                if ( neighbourX >= 0 && neighbourX < size.x && neighbourY >= 0 && neighbourY < size.y )
                {
                    if ( neighbourX != gridX || neighbourY != gridY )
                        wallCount += map[ neighbourX, neighbourY ];
                }
                else
                    wallCount++;

        return wallCount;
    }

    void DrawMapTiles()
    {
        for ( int x = 0 ; x < size.x ; x++ )
            for ( int y = 0 ; y < size.y ; y++ )
            {
                Vector3 pos = grid.CellToWorld( new Vector3Int( x, y, 0 ) );
                Instantiate( tilePrefabs[ MapBiomes[ x, y ] ][ MapHeights[ x, y ] ], pos, Quaternion.identity, gameObject.transform );
            }

    }
}

[Serializable]
public class Blocks
{
    public GameObject normalBlock;
    public GameObject highBlock;

    public GameObject this[ int i ]
    {
        get { return ( i == 0 ) ? normalBlock : highBlock; }
    }
}