using System;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader<T> where T : IKeyedItem
{
    public List<T> ItemsList { get; private set; }
    public Dictionary<int, T> ItemsDict { get; private set; }

    public DataLoader(string path)
    {
        string jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, T>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.Key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<T> Items;
    }

    public T GetByKey(int key) => ItemsDict.TryGetValue(key, out var item) ? item : default;
    public T GetByIndex(int index) => (index >= 0 && index < ItemsList.Count) ? ItemsList[index] : default;
}

public interface IKeyedItem
{
    int Key { get; }
}