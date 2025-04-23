using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ConsumeItem
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 효과
    /// </summary>
    public int Effect;

    /// <summary>
    /// 최대 보유 가능 개수
    /// </summary>
    public int MaxNum;

    /// <summary>
    /// 아이템 타입
    /// </summary>
    public DesignEnums.ConsumeItemType ConsumeItemType;

}
public class ConsumeItemLoader
{
    public List<ConsumeItem> ItemsList { get; private set; }
    public Dictionary<int, ConsumeItem> ItemsDict { get; private set; }

    public ConsumeItemLoader(string path = "JSON/ConsumeItem")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ConsumeItem>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ConsumeItem> Items;
    }

    public ConsumeItem GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ConsumeItem GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
