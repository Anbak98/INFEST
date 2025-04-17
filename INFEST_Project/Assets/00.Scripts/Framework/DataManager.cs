using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    public DataLoader<CharacterInfo> CharacterInfoLoader {  get; private set; }
    public DataLoader<CharaterState> CharaterStateLoader { get; private set; }
    public DataLoader<ConsumeItem> ConsumeItemLoader { get; private set; }
    public DataLoader<MonsterInfo> MonsterInfoLoader { get; private set; }
    public DataLoader<MonsterState> MonsterStateLoader { get; private set; }
    public DataLoader<WeaponInfo> WeaponInfoLoader { get; private set; }
    public DataLoader<PlayerData> PlayerDataLoader { get; private set; }


    public void Initialize()
    {
        CharacterInfoLoader = new DataLoader<CharacterInfo>("JSON/CharacterInfo");
        CharaterStateLoader = new DataLoader<CharaterState>("JSON/CharaterState");
        ConsumeItemLoader = new DataLoader<ConsumeItem>("JSON/ConsumeItem");
        MonsterInfoLoader = new DataLoader<MonsterInfo>("JSON/MonsterInfo");
        MonsterStateLoader = new DataLoader<MonsterState>("JSON/MonsterState");
        WeaponInfoLoader = new DataLoader<WeaponInfo>("JSON/WeaponInfo");
        PlayerDataLoader = new DataLoader<PlayerData>("JSON/PlayerData");
    }
}
