////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif SceneManager File
////////////////////////////////////////////////////////////////////////////////////////////////////


// 오브젝트 데이터
struct MapObject
{
	string name;

	float worldX, worldY, worldZ;
	int   cellX,  cellY,  cellZ;
};

// 네비메시 데이터
struct NavMeshData
{
	struct Vertex   { float x, y, z; };
	struct Triangle { int   a, b, c; };

	vector< Vertex > vertices;
	vector< Triangle > triangles;
	vector< int > areas;
};

// 전체 맵 데이터
struct SceneMap
{
	vector< MapObject > objects;
	NavMeshData navmesh;
};

struct HexNode
{
	int id = 0;
	int x, y;
	std::vector<std::pair<int, int>> neighbors;
	bool walkable = true;
};

class HexGraph
{
public:
	// (x,y) → HexNode
	std::map<std::pair<int, int>, HexNode> nodes;

	bool HasNode( int x, int y ) const
	{
		return nodes.find( { x, y } ) != nodes.end();
	}

	const HexNode* GetNode( int x, int y ) const
	{
		auto it = nodes.find( { x, y } );
		return ( it != nodes.end() ) ? &it->second : nullptr;
	}
};

struct PathNode
{
	int x, y;
	float g;   // 시작점 → 현재까지 비용
	float h;   // 휴리스틱 (목표까지 추정 비용)
	std::pair<int, int> parent;
	float f() const { return g + h; }
};

////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif SceneManager class
////////////////////////////////////////////////////////////////////////////////////////////////////
class SceneManager
{
private:
	/// 격자 타입 정의
	using Grid = std::vector< std::string >;

	/// 격자 좌표 타입 정의
	using GridPos = std::pair< int, int >;

	/// 격자 좌표를 키로 가지는 노드Id 맵 타입 정의
	using NodeIdMapByGridPos = std::map< GridPos, int >;

	/// 노드Id를 키로 가지는 격자 좌표 맵 타입 정의
	using GridPosMapByNodeId = std::map< int, GridPos >;

	/// 최단 경로 노드 목록 타입 정의
	using AStarPath = std::set< int >;

private:
	int   m_width  = 0;
	int   m_height = 0;
	float minX = 99999, maxX = -99999;
	float minY = 99999, maxY = -99999;

	/// Map
	SceneMap m_sceneMap;

	/// 격자 Map
	Grid m_grid;

	/// Graph
	HexGraph m_graph;

	/// 격자 좌표를 키로 가지는 노드Id 맵 타입 (x,y) → ID
	NodeIdMapByGridPos m_idMap;

	/// 노드Id를 키로 가지는 격자 좌표 맵 타입 ID → (x,y)
	GridPosMapByNodeId m_coordMap;

	/// 가장 최근에 조회한 최단 경로의 노드 모음
	AStarPath m_nodePathSet;

public:
	/// 생성자
	SceneManager( const string& path );

	/// 맵을 콘솔에 표시해준다.
	void DrawSceneMap( bool isPrintPath = false );

	/// 그래프를 콘솔에 표시해준다.
	void DrawGraph( bool isPrintList = false, bool isPrintPath = true );

public:
	/// 목적지까지의 경로를 구한다.
	AStarPath FindPath( int startId, int goalId );

	/// 월드 좌표 → 노드 ID 변환
	int GetNodeIdByWorldPos( float wx, float wz, float cellSize = 1.0f ) const;

private:
	/// NavMesh 데이터를 추출한다.
	void _ImportTo( const string& path );

	/// 격자 그리드를 생성한다.
	void _BuildGrid();

	/// NavMesh 데이터를 그래프로 변환한다.
	void _ConverToGraph();

	/// NavMesh 삼각형 안에 점이 포함되는지 체크 (단순 2D 판정)
	bool _PointInTriangle2D( float px, float pz,
							float ax, float az,
							float bx, float bz,
							float cx, float cz );
};
