using UnityEngine;

public class UIManager
{
    public T Show<T>() where T : UIBase
    {
        var ui = Global.Instance.ResourceManager.LoadUI<T>();
        return Object.Instantiate(ui);
    }
}