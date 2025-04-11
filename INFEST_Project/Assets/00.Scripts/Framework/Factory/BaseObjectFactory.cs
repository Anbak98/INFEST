using UnityEngine;

public abstract class BaseObjectFactory<T> where T : ItemObject
{
    private GameObject prefab;

    public BaseObjectFactory()
    {
        prefab = Resources.Load<GameObject>(typeof(T).Name);
    }

    public GameObject CreateInstance()
    {
        GameObject newInstance = GameObject.Instantiate(prefab);
        return newInstance;
    }
}
