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

    /// <summary> ������Ʈ Ǯ �ʱ�ȭ (������ ���) </summary>
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

    /// <summary> ������Ʈ Ǯ���� �������� </summary>
    public GameObject Get(string prefabName, Transform parent = null)
    {
        if (_pools.ContainsKey(prefabName))
        {
            var obj = _pools[prefabName].Get();

            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
            }

            return obj;
        }

        Debug.LogError($"[ObjectPoolManager] '{prefabName}' �������� ã�� �� �����ϴ�!");
        return null;
    }

    public T Get<T>(string prefabName, Transform parent = null) where T : Component
    {
        if (_pools.ContainsKey(prefabName))
        {
            var obj = _pools[prefabName].Get();

            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
            }

            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"[ObjectPoolManager] '{prefabName}' ������Ʈ�� {typeof(T)} ������Ʈ�� �����ϴ�!");
            }

            return component;
        }

        Debug.LogError($"[ObjectPoolManager] '{prefabName}' �������� ã�� �� �����ϴ�!");
        return null;
    }

    /// <summary> ������Ʈ ��ȯ </summary>
    public void Return(GameObject obj)
    {
        var poolObj = obj.GetComponent<PoolObject>();
        if (poolObj == null)
        {
            Debug.LogError($"PoolObject ������Ʈ�� ���� ������Ʈ �Դϴ�!");
            DestroyImmediate(obj);
            return;
        }

        if (!_pools.ContainsKey(poolObj.poolName))
        {
            Debug.LogError($"{obj.name} has not pool");
            DestroyImmediate(obj);
            return;
        }

        _pools[poolObj.poolName].Return(obj);
    }
}
