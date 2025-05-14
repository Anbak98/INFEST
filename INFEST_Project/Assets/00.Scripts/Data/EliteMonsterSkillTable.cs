using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EliteMonsterSkillTable : IKeyedItem
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
    /// 데미지 계수
    /// </summary>
    public float DamageCoefficient;

    /// <summary>
    /// 쿨다운
    /// </summary>
    public float CoolDown;

    /// <summary>
    /// 스킬 사용 범위
    /// </summary>
    public float UseRange;

    /// <summary>
    /// 웨이브 시 사용 범위
    /// </summary>
    public float UseRangeInWave;

    /// <summary>
    /// 사용 몬스터 인덱스
    /// </summary>
    public int UseMonster;

    /// <summary>
    /// 사용 우선순위
    /// </summary>
    public int SkillPriority;

    public int Key => key;

}
