using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManager : SingletonMonoBehaviour<AssetBundleManager>
{
    private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

    private string assetBundleBasePath;

    public void Initialize(string basePath)
    {
        assetBundleBasePath = basePath;
    }

    public AssetBundle Loadbundle(string bundleName)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            return loadedBundles[bundleName];
        }

        string path = Path.Combine(assetBundleBasePath, bundleName);

        if (!File.Exists(path))
        {
            Debug.LogError($"AssetBundle이 다음 경로 내에 없음 : {path}");
            return null;
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        if (bundle != null)
        {
            loadedBundles.Add(bundleName, bundle);
            Debug.Log($"AssetBundle '{bundleName}' 로드됨.");
        }
        else
        {
            Debug.LogError($"AssetBundle 로드 실패 : {bundleName}");
        }


        return bundle;
    }

    public T LoadAsset<T>(string bundleName, string assetName) where T : Object
    {
        AssetBundle bundle = Loadbundle(bundleName);
        if (bundle == null)
            return null;

        T asset = bundle.LoadAsset<T>(assetName);
        if(asset == null)
        {
            Debug.LogError($"{bundleName}에서 {assetName}을(를) 로드하는데 실패");
        }

        return asset;
    }

    public void UnloadBundle(string bundleName, bool unloadAllLoadedObjects = false)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            loadedBundles[bundleName].Unload(unloadAllLoadedObjects);
            loadedBundles.Remove(bundleName);
            Debug.Log($"AssetBundle '{bundleName}' 언로드됨.");
        }
        else
        {
            Debug.LogWarning($"언로드 실패. '{bundleName}'이 로드되어있지 않음.");
        }
    }
}
