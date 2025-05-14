using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MonsterInfo
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
    /// 등급
    /// </summary>
    public DesignEnums.MonsterType MonsterType;

    /// <summary>
    /// 시작 시 체력
    /// </summary>
    public int MinHealth;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int MaxHealth;

    /// <summary>
    /// 5분당 체력 증가량
    /// </summary>
    public int HealthPer5Min;

    /// <summary>
    /// 쳬력 계수
    /// </summary>
    public float HPCoef;

    /// <summary>
    /// 시작 시 공격력
    /// </summary>
    public int MinAtk;

    /// <summary>
    /// 최대 공격력
    /// </summary>
    public int MaxAtk;

    /// <summary>
    /// 5분당 공격력 증가량
    /// </summary>
    public int AtkPer5Min;

    /// <summary>
    /// 공격력 계수
    /// </summary>
    public float AtkCoef;

    /// <summary>
    /// 시작 시 방어력
    /// </summary>
    public int MinDef;

    /// <summary>
    /// 최대 방어력
    /// </summary>
    public int MaxDef;

    /// <summary>
    /// 5분당 방어력 증가량
    /// </summary>
    public int DefPer5Min;

    /// <summary>
    /// 방어력 계수
    /// </summary>
    public float DefCoef;

    /// <summary>
    /// 이동속도
    /// </summary>
    public float SpeedMove;

    /// <summary>
    /// 이동속도
    /// </summary>
    public float SpeedMoveWave;

    /// <summary>
    /// 공격속도
    /// </summary>
    public float SpeedAtk;

    /// <summary>
    /// 감지범위_일반상태
    /// </summary>
    public float DetectAreaNormal;

    /// <summary>
    /// 감지범위_웨이브 시작 시
    /// </summary>
    public float DetectAreaWave;

    /// <summary>
    /// 상태
    /// </summary>
    public int State;

    /// <summary>
    /// 처치 시 골드
    /// </summary>
    public int DropGold;

    /// <summary>
    /// 필드 스폰 여부
    /// </summary>
    public bool FieldSpawn;

    /// <summary>
    /// 스폰수 제한
    /// </summary>
    public int LimitSpawnCount;

}
