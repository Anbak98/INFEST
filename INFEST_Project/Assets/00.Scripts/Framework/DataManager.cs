using System;
using System.Collections.Generic;

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
            {typeof(PlayerData), new DataLoader<PlayerData>("JSON/PlayerData") },
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
}
