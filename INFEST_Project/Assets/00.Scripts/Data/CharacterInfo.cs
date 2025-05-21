using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharacterInfo : IKeyedItem
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 체력
    /// </summary>
    public int Health;

    /// <summary>
    /// 방어구 체력
    /// </summary>
    public int DefGear;

    /// <summary>
    /// 방어력
    /// </summary>
    public int Def;

    /// <summary>
    /// 이동속도
    /// </summary>
    public int SpeedMove;

    /// <summary>
    /// 시작 골드
    /// </summary>
    public int StartGold;

    /// <summary>
    /// 시작 시 팀코인
    /// </summary>
    public int StartTeamCoin;

    /// <summary>
    /// 상태
    /// </summary>
    public int State;

    /// <summary>
    /// 무기1 ID
    /// </summary>
    public int StartWeapon1;

    /// <summary>
    /// 보조무기 ID
    /// </summary>
    public int StartAuxiliaryWeapon;

    /// <summary>
    /// 소비아이템1 ID
    /// </summary>
    public int StartConsumeItem1;

    /// <summary>
    /// 소비아이템1 개수
    /// </summary>
    public int Item1_Count;

    /// <summary>
    /// 소비아이템2 ID
    /// </summary>
    public int StartConsumeItem2;

    /// <summary>
    /// 소비아이템1 개수
    /// </summary>
    public int Item2_Count;
    
    public int Key => key;


}
