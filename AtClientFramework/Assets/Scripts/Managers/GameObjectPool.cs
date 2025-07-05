using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly Stack<GameObject> _pool = new Stack<GameObject>();
    private readonly GameObject _prefab;
    private readonly Transform _parentTransform;

    public GameObjectPool(GameObject prefab, int initialCount = 0, Transform parent = null)
    {
        _prefab = prefab;
        _parentTransform = parent; // 부모 Transform 설정

        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            _pool.Push(obj);
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(_prefab, _parentTransform); // 부모 설정
        obj.SetActive(false);
        return obj;
    }

    public GameObject Get()
    {
        GameObject obj = _pool.Count > 0 ? _pool.Pop() : CreateNewObject();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(_parentTransform); // 풀링된 오브젝트는 부모 오브젝트로 이동
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        _pool.Push(obj);
    }
}
