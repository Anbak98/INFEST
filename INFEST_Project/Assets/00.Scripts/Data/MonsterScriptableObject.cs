using Fusion;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "monster")]
public class MonsterScriptableObject : ScriptableObject
{
    [SerializeField] private NetworkPrefabRef[] normalMonsters;
    [SerializeField] private NetworkPrefabRef[] eliteMonsters;
    [SerializeField] private NetworkPrefabRef[] bossMonsters;

    private readonly Dictionary<int, NetworkPrefabRef> monsterMap = new();

    public void Init()
    {
        int normalIndex = 1;
        foreach (var normal in normalMonsters)
        {
            monsterMap.Add(1000 + normalIndex++, normal);
        }

        int elitelIndex = 1;
        foreach (var elite in eliteMonsters)
        {
            monsterMap.Add(2000 + elitelIndex++, elite);
        }

        int bossIndex = 1;
        foreach (var boos in bossMonsters)
        {
            monsterMap.Add(3000 + bossIndex++, boos);
        }
    }

    public NetworkPrefabRef GetByKey(int key)
    {        
        if(monsterMap.TryGetValue(key, out var monster))
        {
            return monster;
        }

        return default;
    }
}
