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
            {typeof(BossRunPoint), new DataLoader<BossRunPoint>("JSON/BossRunPoint") },
            {typeof(PlayerData), new DataLoader<PlayerData>("JSON/PlayerData") },
            //==========Common Monster==========
            {typeof(CommonSkillTable), new DataLoader<CommonSkillTable>("JSON/CommonSkillTable") },
            
            //===========Elite Monster==========
            {typeof(BowmeterSkillTable), new DataLoader<BowmeterSkillTable>("JSON/BowmeterSkillTable") },
            {typeof(GritaSkillTable), new DataLoader<GritaSkillTable>("JSON/GritaSkillTable") },
            {typeof(GoreHaulSkillTable), new DataLoader<GoreHaulSkillTable>("JSON/GoreHaulSkillTable") },
            //===========Boss Monster============
            {typeof(RageFangSkillTable), new DataLoader<RageFangSkillTable>("JSON/RageFangSkillTable") },
            {typeof(BossEscapeTable), new DataLoader<BossEscapeTable>("JSON/BossEscapeTable") },
            //=======Shop && MysteryBox =========
            {typeof(ShopItemAppearanceTable), new DataLoader<ShopItemAppearanceTable>("JSON/ShopItemAppearanceTable") },
            {typeof(MysteryBoxTable), new DataLoader<MysteryBoxTable>("JSON/MysteryBoxTable") },
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
