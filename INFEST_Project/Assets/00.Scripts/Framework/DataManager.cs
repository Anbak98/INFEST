using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    public CharacterInfoLoader CharacterInfoLoader {  get; private set; }
    public CharaterStateLoader CharaterStateLoader { get; private set; }
    public ConsumeItemLoader ConsumeItemLoader { get; private set; }
    public MonsterInfoLoader MonsterInfoLoader { get; private set; }
    public MonsterStateLoader MonsterStateLoader { get; private set; }
    public WeaponInfoLoader WeaponInfoLoader { get; private set; }
    public PlayerDataLoader PlayerDataLoader { get; private set; }


    public void Initialize()
    {
        CharacterInfoLoader = new CharacterInfoLoader();
        CharaterStateLoader = new CharaterStateLoader();
        ConsumeItemLoader = new ConsumeItemLoader();
        MonsterInfoLoader = new MonsterInfoLoader();
        MonsterStateLoader = new MonsterStateLoader();
        WeaponInfoLoader = new WeaponInfoLoader();
        PlayerDataLoader = new PlayerDataLoader();
    }
}
