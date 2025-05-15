using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DataManager : SingletonBehaviour<DataManager>
{
    private Dictionary<Type, IDataLoader> _dataLoaderMap;

    protected override void Awake()
    {
        base.Awake();
        _dataLoaderMap = new()
        {
            {typeof(CharacterInfo), new DataLoader<CharacterInfo>("JSON/CharacterInfo") },
            {typeof(CharaterState), new DataLoader<CharaterState>("JSON/CharaterState") },
            {typeof(ConsumeItem), new DataLoader<ConsumeItem>("JSON/ConsumeItem") },
            {typeof(MonsterInfo), new DataLoader<MonsterInfo>("JSON/MonsterInfo") },
            {typeof(MonsterState), new DataLoader<MonsterState>("JSON/MonsterState") },
            {typeof(WeaponInfo), new DataLoader<WeaponInfo>("JSON/WeaponInfo") },
            {typeof(SpawnTable), new DataLoader<SpawnTable>("JSON/SpawnTable") },
            {typeof(PlayerData), new DataLoader<PlayerData>("JSON/PlayerData") },
            //==========Common Monster==========
            
            //===========Elite Monster==========
            {typeof(BowmeterSkillTable), new DataLoader<BowmeterSkillTable>("JSON/BowmeterSkillTable") },
            {typeof(GritaSkillTable), new DataLoader<GritaSkillTable>("JSON/GritaSkillTable") },
            {typeof(GoreHaulSkillTable), new DataLoader<GoreHaulSkillTable>("JSON/GoreHaulSkillTable") },
            //===========Boss Monster============
            {typeof(RageFangSkillTable), new DataLoader<RageFangSkillTable>("JSON/RageFangSkillTable") },
        };
    }

    public T GetByKey<T>(int key) where T : IKeyedItem
    {
        if (_dataLoaderMap.TryGetValue(typeof(T), out var dataLoader))
        {
            DataLoader<T> loader = dataLoader as DataLoader<T>;
            return loader.GetByKey(key);
        }

        return default;
    }

    public Dictionary<int, T> GetDictionary<T>() where T : IKeyedItem
    {
        if (_dataLoaderMap.TryGetValue(typeof(T), out var dataLoader))
        {
            DataLoader<T> loader = dataLoader as DataLoader<T>;
            return loader.ItemsDict;
        }

        return default;
    }
}
