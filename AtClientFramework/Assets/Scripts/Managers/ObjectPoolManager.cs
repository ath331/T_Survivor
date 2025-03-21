using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolManager");
                _instance = obj.AddComponent<ObjectPoolManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private bool _isInitialized = false;
    private readonly Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool>();
    private Transform _poolContainer;

    /// <summary> 오브젝트 풀 초기화 (프리셋 기반) </summary>
    public void Initialize()
    {
        if (_isInitialized)
            return;

        GameObject container = new GameObject("PoolContainer");
        Object.DontDestroyOnLoad(container);
        _poolContainer = container.transform;

        var pref = Resources.Load<ObjectPoolingPresets>("ObjectPoolingPresets");
        if (pref != null)
        {
            foreach (var preset in pref.presets)
            {
                if (string.IsNullOrWhiteSpace(preset.name))
                {
                    preset.name = preset.prefab.name;
                }

                Transform poolTr = new GameObject($"{preset.name} Object Pool").transform;
                poolTr.SetParent(_poolContainer, false);

                _pools[preset.name] = new GameObjectPool(preset.prefab, preset.initialCount, poolTr);
            }
        }

        _isInitialized = true;
    }

    /// <summary> 오브젝트 풀에서 가져오기 </summary>
    public GameObject Get(string prefabName)
    {
        if (!_isInitialized)
            Initialize();

        if (_pools.ContainsKey(prefabName))
        {
            return _pools[prefabName].Get();
        }

        Debug.LogError($"[ObjectPoolManager] '{prefabName}' 프리셋을 찾을 수 없습니다!");
        return null;
    }

    public T Get<T>(string prefabName, Transform parent = null) where T : Component
    {
        GameObject obj = Get(prefabName);

        if (obj != null)
        {
            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
            }

            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"[ObjectPoolManager] '{prefabName}' 오브젝트에 {typeof(T)} 컴포넌트가 없습니다!");
            }

            return component;
        }

        return null;
    }

    /// <summary> 오브젝트 반환 </summary>
    public void Return(string prefabName, GameObject obj)
    {
        if (_pools.ContainsKey(prefabName))
        {
            _pools[prefabName].Return(obj);
        }
        else
        {
            Debug.LogWarning($"[ObjectPoolManager] 해당 프리팹({prefabName})의 풀을 찾을 수 없습니다!");
            GameObject.Destroy(obj);
        }
    }
}
