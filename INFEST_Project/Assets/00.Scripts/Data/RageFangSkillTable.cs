using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RageFangSkillTable : IKeyedItem
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
    /// 1m이내일 경우
    /// </summary>
    public float Distance_1P1M;

    /// <summary>
    /// 3m이내일 경우
    /// </summary>
    public float Distance_1P3M;

    /// <summary>
    /// 5m이내일경우
    /// </summary>
    public float Distance_1P5M;

    /// <summary>
    /// 1m이내일 경우
    /// </summary>
    public float Distance_2P1M;

    /// <summary>
    /// 3m이내일 경우
    /// </summary>
    public float Distance_2P3M;

    /// <summary>
    /// 5m이내일경우
    /// </summary>
    public float Distance_2P5M;

    public int Key => key;

}
