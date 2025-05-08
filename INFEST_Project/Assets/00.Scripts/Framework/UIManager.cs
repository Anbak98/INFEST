using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    public Dictionary<string, UIBase> UIList = new();

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;

        if (UIList.TryGetValue(uiName, out var existingUI))
        {
            existingUI.gameObject.SetActive(true);
            return existingUI as T;
        }

        var ui = Resources.Load<UIBase>($"UI/{typeof(T).Name}") as T;

        var instantiated = Object.Instantiate(ui);
        UIList.Add(uiName, instantiated);
        return instantiated;
    }

    //public T Load<T>(string uiName) where T : UIBase
    //{
    //    if (UIList.ContainsKey(uiName))
    //    {
    //        return UIList[uiName] as T;
    //    }

    //    var newCanvasObject = new GameObject(uiName + " Canvas");

    //    var canvas = newCanvasObject.AddComponent<Canvas>();
    //    canvas.renderMode = RenderMode.ScreenSpaceOverlay;

    //    var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
    //    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;  
    //    canvasScaler.referenceResolution = new Vector2 (1920, 1080);

    //    newCanvasObject.AddComponent<GraphicRaycaster>();

    //    var prefab = Resources.Load<UIBase>($"UI/{uiName}");
    //    var obj = Object.Instantiate(prefab, newCanvasObject.transform);
    //    obj.name = obj.name.Replace("(Clone)", "");

    //    var result = obj.GetComponent<T>();
    //    result.canvas.sortingOrder = UIList.Count;
                
    //    return result;
    //}

    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        
        if(UIList.TryGetValue(uiName, out var ui))
        {
            ui.gameObject.SetActive(false);
        }
    }

    public T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;

        if (UIList.TryGetValue(uiName, out var ui))
        {
            return ui as T;
        }

        return null;
    }
}