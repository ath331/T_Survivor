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


////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif SceneManager class
////////////////////////////////////////////////////////////////////////////////////////////////////
class SceneManager
{
private:
	/// Map
	SceneMap m_sceneMap;

public:
	/// 생성자
	SceneManager( const string& path );

	/// 맵을 콘솔에 표시해준다.
	void DrawSceneMap();

private:
	// NavMesh 삼각형 안에 점이 포함되는지 체크 (단순 2D 판정)
	bool _PointInTriangle2D( float px, float pz,
							float ax, float az,
							float bx, float bz,
							float cx, float cz );
};
