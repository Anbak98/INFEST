using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public Dictionary<string, UIBase> UIList = new();

    public T Show<T>() where T : UIBase
    {
        if (UIList.ContainsKey(typeof(T).Name))
        {
            Debug.Log("[ResourceManager] Try loading UI that already loaded");
            return UIList[typeof(T).Name] as T;
        }

        var ui = Resources.Load<UIBase>($"UI/{typeof(T).Name}") as T;
        UIList.Add(ui.name, ui);
        return Object.Instantiate(ui);
    }

    public void Hide<T>() where T : UIBase
    {

    }
}