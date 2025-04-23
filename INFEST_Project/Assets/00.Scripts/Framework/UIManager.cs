using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public T Load<T>(string uiName) where T : UIBase
    {
        var newCanvasObject = new GameObject(uiName + " Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;  
        canvasScaler.referenceResolution = new Vector2 (1920, 1080);

        newCanvasObject.AddComponent<GraphicRaycaster>();

        var prefab = Resources.Load<UIBase>($"UI/{uiName}");
        var obj = Object.Instantiate(prefab, newCanvasObject.transform);
        obj.name = obj.name.Replace("(Clone)", "");

        var result = obj.GetComponent<T>();
        result.canvas.sortingOrder = UIList.Count;

        return result;
    }

    public void Hide<T>()
    {
        string uiName = typeof(T).ToString();

    }
}