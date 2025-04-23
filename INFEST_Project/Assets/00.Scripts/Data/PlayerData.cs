using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// index
    /// </summary>
    public string Playeridx;

    /// <summary>
    /// 닉네임
    /// </summary>
    public string NickName;

}
public class PlayerDataLoader
{
    public List<PlayerData> ItemsList { get; private set; }
    public Dictionary<int, PlayerData> ItemsDict { get; private set; }

    public PlayerDataLoader(string path = "JSON/PlayerData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, PlayerData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<PlayerData> Items;
    }

    public PlayerData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public PlayerData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
