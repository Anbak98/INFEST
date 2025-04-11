using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MonsterState
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
public class MonsterStateLoader
{
    public List<MonsterState> ItemsList { get; private set; }
    public Dictionary<int, MonsterState> ItemsDict { get; private set; }

    public MonsterStateLoader(string path = "JSON/MonsterState")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, MonsterState>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<MonsterState> Items;
    }

    public MonsterState GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public MonsterState GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
