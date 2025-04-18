using UnityEngine;

public static class UnityExtension
{
    public static T AddGetComponent<T>(this GameObject gameObject) where T : Component
    {
        T result = gameObject.GetComponent<T>();

        return result ?? gameObject.AddComponent<T>();
    }
}
