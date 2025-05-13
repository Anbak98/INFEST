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
        CreateInstance();

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
        // 쓰레드 락 (잡 시스템) FindObjectsOfType
        if( _instance == null)
        {
            T[] instances = FindObjectsOfType<T>();

            if (instances.Length > 0)
            {
                _instance = instances[0];


                for (int i = 1; i < instances.Length; i++)
                {
                    if (Application.isPlaying)
                        Destroy(instances[i].gameObject);
                    else
                        DestroyImmediate(instances[i].gameObject);
                }

            }
            else
            {
                GameObject obj = new GameObject(string.Format(typeof(T).Name + "_auto"));
                _instance = obj.AddComponent<T>();
            }
        }
    }
}
