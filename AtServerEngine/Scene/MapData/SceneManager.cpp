////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif SceneManager File
////////////////////////////////////////////////////////////////////////////////////////////////////


#include "pch.h"
#include "SceneManager.h"
#include <iomanip>


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 생성자
////////////////////////////////////////////////////////////////////////////////////////////////////
SceneManager::SceneManager( const string& path )
{
	_ImportTo( path );
	_ConverToGraph();
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 맵을 콘솔에 표시해준다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::DrawSceneMap( bool isPrintPath )
{
    if ( m_grid.empty() )
    {
        cout << "Grid is Empty()" << endl;
        return;
	}
    
	// 콘솔 출력 (육각형 오프셋 적용)
	for ( int z = 0; z < m_height; ++z )
	{
		// 홀수 행은 왼쪽에 공백을 추가
		if ( z % 2 == 1 )
			std::cout << " ";

		for ( int x = 0; x < m_width; ++x )
		{
			if ( isPrintPath )
			{
				// TODO 경로 빨간색 별로 표시하기?
				std::cout << m_grid[ z ][ x ];
			}
			else
			{
				std::cout << m_grid[ z ][ x ];
			}

			std::cout << " "; // 각 원소마다 공백 추가
		}

		std::cout << std::endl;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 그래프를 콘솔에 표시해준다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::DrawGraph( bool isPrintList, bool isPrintPath )
{
	if ( m_graph.nodes.empty() )
	{
		std::cout << "Graph is empty.\n";
		return;
	}

	int maxId = (int)m_graph.nodes.size();
	int widthDigits = ( maxId > 0 ) ? (int)std::log10( maxId ) + 1 : 1;
	if ( widthDigits < 2 ) widthDigits = 2;

	std::cout << "\n\nHexGraph (Node Id)\n";

	for ( int y = 0; y < m_height; y++ )
	{
		if ( y % 2 == 1 ) std::cout << " ";  // 홀수 행 오프셋

		for ( int x = 0; x < m_width; x++ )
		{
			auto it = m_idMap.find( { x, y } );
			if ( it == m_idMap.end() )
			{
				for ( int k = 0; k < widthDigits; k++ ) std::cout << ".";
				std::cout << " ";
			}
			else
			{
				if ( isPrintPath )
				{
					// 표시하려는 노드가 최근에 조회한 경로라면
					if ( m_nodePathSet.find( it->second ) != m_nodePathSet.end() )
					{
						std::cout << "\033[31m"  // 빨간색 시작
							<< std::setw( widthDigits ) << std::setfill( '0' ) << it->second
							<< "\033[0m "; // 색상 리셋
					}
					else
					{
						std::cout << std::setw( widthDigits )
							<< std::setfill( '0' )
							<< it->second << " ";
					}
				}
				else
				{
					std::cout << std::setw( widthDigits )
						<< std::setfill( '0' )
						<< it->second << " ";
				}
			}
		}
		std::cout << "\n";
	}

	if ( isPrintList )
	{
		std::cout << "\n인접 리스트:\n";
		for ( auto& kv : m_graph.nodes )
		{
			int id = m_idMap[ {kv.second.x, kv.second.y} ];
			std::cout << "Node " << id
				<< " (" << kv.second.x << "," << kv.second.y << ") -> ";

			for ( auto& nb : kv.second.neighbors )
			{
				auto it = m_idMap.find( nb );
				if ( it != m_idMap.end() )
					std::cout << it->second << " ";
			}
			std::cout << "\n";
		}
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 목적지까지의 경로를 구한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
SceneManager::AStarPath SceneManager::FindPath( int startId, int goalId )
{
	m_nodePathSet.clear();

	GridPos start = m_coordMap[ startId ];
	GridPos goal  = m_coordMap[ goalId ];

	if ( !m_graph.HasNode( start.first, start.second ) ||
		 !m_graph.HasNode( goal.first, goal.second ) )
		return {};

	auto& startNode = *m_graph.GetNode( start.first, start.second );
	auto& goalNode = *m_graph.GetNode( goal.first, goal.second );

	auto heuristic = [ & ]( const HexNode& a, const HexNode& b )
	{
		float dx = float( a.x - b.x );
		float dy = float( a.y - b.y );
		return std::sqrt( dx * dx + dy * dy );
	};

	using PQItem = std::pair< float, GridPos >; // f-score, 좌표
	std::priority_queue< PQItem, std::vector< PQItem >, std::greater<PQItem>> open;

	auto key = []( int x, int y )
	{
		return ( (long long)x << 32 ) ^ (long long)y;
	};

	std::unordered_map< long long, PathNode > allNodes;
	std::unordered_map< long long, bool > closed;

	// 시작 노드 초기화
	PathNode s;
	s.x = start.first;
	s.y = start.second;
	s.g = 0.0f;
	s.h = heuristic( startNode, goalNode );
	s.parent = { -1, -1 };

	allNodes[ key( s.x, s.y ) ] = s;
	open.emplace( s.f(), start );

	while ( !open.empty() )
	{
		auto [fscore, current] = open.top();
		open.pop();

		int cx = current.first;
		int cy = current.second;
		auto ckey = key( cx, cy );

		if ( closed[ ckey ] ) continue;
		closed[ ckey ] = true;

		if ( cx == goal.first && cy == goal.second )
		{
			// 경로 복원
			AStarPath path;
			GridPos cur = goal;
			while ( !( cur.first == -1 && cur.second == -1 ) )
			{
                auto idMapKey = std::make_pair( cur.first, cur.second );
                auto iter = m_idMap.find( idMapKey );
                if ( iter == m_idMap.end() )
                    continue;

                path.insert( iter->second );
				m_nodePathSet.insert( iter->second );

				cur = allNodes[ key( cur.first, cur.second ) ].parent;
			}

			//std::reverse( path.begin(), path.end() );

			return path;
		}

		const HexNode* curNode = m_graph.GetNode( cx, cy );
		if ( !curNode ) continue;

		for ( auto& nb : curNode->neighbors )
		{
			if ( !m_graph.HasNode( nb.first, nb.second ) ) continue;
			const HexNode* nbNode = m_graph.GetNode( nb.first, nb.second );
			if ( !nbNode->walkable ) continue;

			float tentativeG = allNodes[ ckey ].g + heuristic( *curNode, *nbNode );
			auto nkey = key( nb.first, nb.second );

			if ( !allNodes.count( nkey ) || tentativeG < allNodes[ nkey ].g )
			{
				PathNode pn;
				pn.x = nb.first;
				pn.y = nb.second;
				pn.g = tentativeG;
				pn.h = heuristic( *nbNode, goalNode );
				pn.parent = { cx, cy };

				allNodes[ nkey ] = pn;
				open.emplace( pn.f(), nb );
			}
		}
	}

	return {}; // 경로 없음
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 월드 좌표 → 노드 ID 변환
////////////////////////////////////////////////////////////////////////////////////////////////////
int SceneManager::GetNodeIdByWorldPos( float wx, float wz/*, float cellWidth, float cellHeight*/ ) const
{
	//if ( m_width == 0 || m_height == 0 )
	//	return -1;
	//
	//// 격자 y
	//int gy = (int)( ( wz - minY ) / ( cellHeight * 0.75f ) );
	//if ( gy < 0 || gy >= m_height ) return -1;
	//
	//// 홀수 행 보정
	//float offsetX = ( gy % 2 == 1 ) ? ( cellWidth / 2.0f ) : 0.0f;
	//
	//// 격자 x
	//int gx = (int)( ( wx - minX - offsetX ) / cellWidth );
	//if ( gx < 0 || gx >= m_width ) return -1;
	//
	//// 노드 ID 반환
	//auto it = m_idMap.find( { gx, gy } );
	//if ( it == m_idMap.end() )
	//	return -1;
	//
	//return it->second;

	int gx = (int)std::floor( wx - m_centerX + ( m_width / 2 ) );
	int gy = (int)std::floor( wz - m_centerY + ( m_height / 2 ) );

	if ( gx < 0 || gx >= m_width || gy < 0 || gy >= m_height )
		return -1;

	auto it = m_idMap.find( { gx, gy } );
	return ( it == m_idMap.end() ) ? -1 : it->second;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 노드 ID -> 월드 좌표
////////////////////////////////////////////////////////////////////////////////////////////////////
std::pair<float, float> SceneManager::GetWorldPosByNodeId( int nodeId/*, float cellWidth, float cellHeight*/ ) const
{
	//auto it = m_coordMap.find( nodeId );
	//if ( it == m_coordMap.end() )
	//	return { -1.0f, -1.0f }; // 없는 노드
	//
	//int gx = it->second.first;
	//int gy = it->second.second;
	//
	//// 홀수/짝수 행 오프셋
	//float offsetX = ( gy % 2 == 1 ) ? ( cellWidth / 2.0f ) : 0.0f;
	//
	//float worldX = minX + gx * cellWidth + offsetX;
	//float worldZ = minY + gy * ( cellHeight * 0.75f );
	//
	//return { worldX, worldZ };

	auto it = m_coordMap.find( nodeId );
	if ( it == m_coordMap.end() )
		return { -1.f, -1.f };

	int gx = it->second.first;
	int gy = it->second.second;

	float worldX = ( gx - m_width / 2 ) + 0.5f +  m_centerX;
	float worldZ = ( gy - m_height / 2 ) + 0.5f + m_centerY;

	return { worldX, worldZ };
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif NavMesh 데이터를 추출한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::_ImportTo( const string& path )
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

	for ( auto& v : m_sceneMap.navmesh.vertices )
	{
		minX = std::min( minX, v.x );
		maxX = std::max( maxX, v.x );
		minY = std::min( minY, v.z );
		maxY = std::max( maxY, v.z );
	}

	m_width  = static_cast<int>( maxX - minX ) + 1;
	m_height = static_cast<int>( maxY - minY ) + 1;

	m_centerX = ( minX + maxX ) / 2.0f;
	m_centerY = ( minY + maxY ) / 2.0f;

    _BuildGrid();
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif 격자 그리드를 생성한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::_BuildGrid()
{
    if ( m_grid.empty() )
        m_grid.clear();

    m_grid.resize( m_height, std::string( m_width, '.' ) );

    // 오브젝트 표시
    for ( auto& obj : m_sceneMap.objects )
    {
        int gx = static_cast<int>( obj.worldX - minX );
        int gz = static_cast<int>( obj.worldZ - minY );
        if ( gz >= 0 && gz < m_height && gx >= 0 && gx < m_width )
            m_grid[ m_height - 1 - gz ][ gx ] = '#';
    }

    // 네비메시 영역 표시
    for ( auto& tri : m_sceneMap.navmesh.triangles )
    {
        auto& a = m_sceneMap.navmesh.vertices[ tri.a ];
        auto& b = m_sceneMap.navmesh.vertices[ tri.b ];
        auto& c = m_sceneMap.navmesh.vertices[ tri.c ];

        for ( int z = 0; z < m_height; ++z )
        {
            for ( int x = 0; x < m_width; ++x )
            {
                //float wx = minX + x + 0.5f;
                //float wz = minY + z + 0.5f;
				float wx = ( x - m_width / 2 ) + 0.5f +  m_centerX;
				float wz = ( z - m_height / 2 ) + 0.5f + m_centerY;

                if ( _PointInTriangle2D( wx, wz, a.x, a.z, b.x, b.z, c.x, c.z ) )
                {
                    if ( m_grid[ m_height - 1 - z ][ x ] == '.' )
                        m_grid[ m_height - 1 - z ][ x ] = '*';
                }
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif NavMesh 데이터를 그래프로 변환한다.
////////////////////////////////////////////////////////////////////////////////////////////////////
void SceneManager::_ConverToGraph()
{
	int id = 0;

	for ( int y = 0; y < m_height; y++ )
	{
		for ( int x = 0; x < m_width; x++ )
		{
			if ( m_grid[ y ][ x ] != '*' ) continue;

			HexNode node;
			node.id = ++id;
			node.x = x;
			node.y = y;   // ✅ grid 좌표 그대로 사용

			std::vector<std::pair<int, int>> candidates;

			if ( y % 2 == 0 ) // 짝수 행
			{
				candidates = {
					{ x - 1, y }, { x + 1, y },
					{ x, y - 1 }, { x, y + 1 },
					{ x - 1, y - 1 }, { x - 1, y + 1 }
				};
			}
			else // 홀수 행
			{
				candidates = {
					{ x - 1, y }, { x + 1, y },
					{ x, y - 1 }, { x, y + 1 },
					{ x + 1, y - 1 }, { x + 1, y + 1 }
				};
			}

			for ( auto& nb : candidates )
			{
				int nx = nb.first;
				int ny = nb.second;
				if ( nx < 0 || nx >= m_width || ny < 0 || ny >= m_height ) continue;

				if ( m_grid[ ny ][ nx ] == '*' )
					node.neighbors.push_back( nb );
			}

			m_graph.nodes[ {x, y} ] = node;
		}
	}

	for ( auto& kv : m_graph.nodes )
	{
		m_idMap[ {kv.second.x, kv.second.y} ] = kv.second.id;
		m_coordMap[ kv.second.id ] = { kv.second.x, kv.second.y };
	}

	std::cout << "\n\nHexGraph built: " << m_graph.nodes.size() << " nodes\n";
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

