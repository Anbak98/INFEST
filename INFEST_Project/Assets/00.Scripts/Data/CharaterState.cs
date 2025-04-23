using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharaterState
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 상태
    /// </summary>
    public string State;

    /// <summary>
    /// 설명
    /// </summary>
    public string Discription;

}
public class CharaterStateLoader
{
    public List<CharaterState> ItemsList { get; private set; }
    public Dictionary<int, CharaterState> ItemsDict { get; private set; }

    public CharaterStateLoader(string path = "JSON/CharaterState")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, CharaterState>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<CharaterState> Items;
    }

    public CharaterState GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public CharaterState GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
