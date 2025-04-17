using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                CreateInstance();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T).Name} found. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private static void CreateInstance()
    {
        T[] instances = FindObjectsOfType<T>();

        if (_instance == null)
        {
            Debug.LogError($"[Singleton] Instance of {typeof(T).Name} not found in the scene.");
            return;
        }

        for (int i = 1; i < instances.Length; i++)
        {
            Destroy(instances[i]);
        }

        _instance = instances[0];
    }
}
