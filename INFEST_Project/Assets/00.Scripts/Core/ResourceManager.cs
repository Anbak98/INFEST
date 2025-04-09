using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, UIBase> UIList = new();

    public T LoadUI<T>() where T : UIBase
    {
        if(UIList.ContainsKey(typeof(T).Name))
        {
            Debug.Log("[ResourceManager] Try loading UI that already loaded");
            return UIList[typeof(T).Name] as T;
        }

        var ui = Resources.Load<UIBase>($"UI/{typeof(T).Name}") as T;
        UIList.Add(ui.name, ui);
        return ui;
    }
}
