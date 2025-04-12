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
        _parentTransform = parent; // �θ� Transform ����

        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            _pool.Push(obj);
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(_prefab, _parentTransform); // �θ� ����
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
        obj.transform.SetParent(_parentTransform); // Ǯ���� ������Ʈ�� �θ� ������Ʈ�� �̵�
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        _pool.Push(obj);
    }
}
