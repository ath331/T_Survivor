////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif SceneManager File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "SceneManager.h"
#include <iostream>
#include <fstream>
#include <json.hpp>


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
SceneManager::SceneManager( const string& path )
{
    std::ifstream file( path );
    if ( !file.is_open() )
        throw std::runtime_error( "파일 열기 실패: " + path );

    nlohmann::json j;
    file >> j;

    // objects
    for ( auto& obj : j[ "objects" ] )
    {
        MapObject o;
        o.name   = obj[ "name"   ].get<std::string>();
        o.worldX = obj[ "worldX" ].get<float>();
        o.worldY = obj[ "worldY" ].get<float>();
        o.worldZ = obj[ "worldZ" ].get<float>();
        o.cellX  = obj[ "cellX"  ].get<int>();
        o.cellY  = obj[ "cellY"  ].get<int>();
        o.cellZ  = obj[ "cellZ"  ].get<int>();
        m_sceneMap.objects.push_back( o );
    }

    // navmesh.vertices
    for ( auto& v : j[ "navmesh" ][ "vertices" ] )
    {
        NavMeshData::Vertex vert;
        vert.x = v[ 0 ].get<float>();
        vert.y = v[ 1 ].get<float>();
        vert.z = v[ 2 ].get<float>();
        m_sceneMap.navmesh.vertices.push_back( vert );
    }

    // navmesh.triangles
    for ( auto& t : j[ "navmesh" ][ "triangles" ] )
    {
        NavMeshData::Triangle tri;
        tri.a = t[ 0 ].get<int>();
        tri.b = t[ 1 ].get<int>();
        tri.c = t[ 2 ].get<int>();
        m_sceneMap.navmesh.triangles.push_back( tri );
    }

    // navmesh.areas
    for ( auto& a : j[ "navmesh" ][ "areas" ] )
    {
        m_sceneMap.navmesh.areas.push_back( a.get<int>() );
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 맵을 콘솔에 표시해준다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::DrawSceneMap()
{
	// 맵 범위 구하기
	float minX = 99999, maxX = -99999;
	float minZ = 99999, maxZ = -99999;

	for ( auto& v : m_sceneMap.navmesh.vertices )
	{
		minX = std::min( minX, v.x );
		maxX = std::max( maxX, v.x );
		minZ = std::min( minZ, v.z );
		maxZ = std::max( maxZ, v.z );
	}
	for ( auto& obj : m_sceneMap.objects )
	{
		minX = std::min( minX, obj.worldX );
		maxX = std::max( maxX, obj.worldX );
		minZ = std::min( minZ, obj.worldZ );
		maxZ = std::max( maxZ, obj.worldZ );
	}

	int width = static_cast<int>( maxX - minX ) + 1;
	int height = static_cast<int>( maxZ - minZ ) + 1;

	std::vector<std::string> grid( height, std::string( width, '.' ) );

	// 오브젝트 표시
	for ( auto& obj : m_sceneMap.objects )
	{
		int gx = static_cast<int>( obj.worldX - minX );
		int gz = static_cast<int>( obj.worldZ - minZ );
		if ( gz >= 0 && gz < height && gx >= 0 && gx < width )
			grid[ height - 1 - gz ][ gx ] = '#';
	}

	// 네비메시 영역 표시
	for ( auto& tri : m_sceneMap.navmesh.triangles )
	{
		auto& a = m_sceneMap.navmesh.vertices[ tri.a ];
		auto& b = m_sceneMap.navmesh.vertices[ tri.b ];
		auto& c = m_sceneMap.navmesh.vertices[ tri.c ];

		for ( int z = 0; z < height; ++z )
		{
			for ( int x = 0; x < width; ++x )
			{
				float wx = minX + x + 0.5f;
				float wz = minZ + z + 0.5f;

				if ( _PointInTriangle2D( wx, wz, a.x, a.z, b.x, b.z, c.x, c.z ) )
				{
					if ( grid[ height - 1 - z ][ x ] == '.' )
						grid[ height - 1 - z ][ x ] = '*';
				}
			}
		}
	}

	// 콘솔 출력 (육각형 오프셋 적용)
	for ( int z = 0; z < height; ++z )
	{
		// 홀수 행은 왼쪽에 공백을 추가
		if ( z % 2 == 1 )
			std::cout << " ";

		for ( int x = 0; x < width; ++x )
		{
			std::cout << grid[ z ][ x ] << " "; // 각 원소마다 공백 추가
		}
		std::cout << std::endl;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif NavMesh 삼각형 안에 점이 포함되는지 체크 (단순 2D 판정)
////////////////////////////////////////////////////////////////////////////////////////////////////
bool SceneManager::_PointInTriangle2D( float px, float pz,
									  float ax, float az,
									  float bx, float bz,
									  float cx, float cz )
{
    float v0x = cx - ax, v0z = cz - az;
    float v1x = bx - ax, v1z = bz - az;
    float v2x = px - ax, v2z = pz - az;

    float dot00 = v0x * v0x + v0z * v0z;
    float dot01 = v0x * v1x + v0z * v1z;
    float dot02 = v0x * v2x + v0z * v2z;
    float dot11 = v1x * v1x + v1z * v1z;
    float dot12 = v1x * v2x + v1z * v2z;

    float invDenom = 1.0f / ( dot00 * dot11 - dot01 * dot01 );
    float u = ( dot11 * dot02 - dot01 * dot12 ) * invDenom;
    float v = ( dot00 * dot12 - dot01 * dot02 ) * invDenom;

    return ( u >= 0 ) && ( v >= 0 ) && ( u + v < 1 );
}
