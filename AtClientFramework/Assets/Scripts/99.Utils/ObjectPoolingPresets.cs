using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolingPresets", menuName = "Pooling/ObjectPoolingPresets")]
public class ObjectPoolingPresets : ScriptableObject
{
    public PoolPreset[] presets;
}

[System.Serializable]
public class PoolPreset
{
    public string name;
    public GameObject prefab;
    public int initialCount = 10;
}
