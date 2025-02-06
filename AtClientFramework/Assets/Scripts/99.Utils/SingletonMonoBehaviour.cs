using UnityEngine;

public class Singleton<T> where T : class, new()
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
            }

            if (instance == null)
            {
                Debug.LogWarning($"No instance of {typeof(T)}, a temporary one is created.");
                instance = new GameObject($"Temp Instance of {typeof(T)}", typeof(T)).GetComponent<T>();
            }

            if (instance == null)
            {
                Debug.LogError($"Problem during the creation of {typeof(T)}");
            }

            return instance;
        }

    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        if (null != instance && (this as T) != instance)
        {
            Debug.LogError("Destory for SingletonMonoBehaviour");

            Destroy(this);
        }
        else
        {
            instance = this as T;

            instance.Init();
        }
    }

    public virtual void Init() { }

    private void OnApplicationQuit()
    {
        instance = null;
    }
}

