using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RageFangSkillTable
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
    /// 1P_3m이내
    /// </summary>
    public int 1P1M;

    /// <summary>
    /// 1P_5m이내
    /// </summary>
    public int 1P3M;

    /// <summary>
    /// 1P_7m이내
    /// </summary>
    public int 1P5M;

    /// <summary>
    /// 2P_3m이내
    /// </summary>
    public int 2P1M;

    /// <summary>
    /// 2P_5m이내
    /// </summary>
    public int 2P3M;

    /// <summary>
    /// 2P_7m이내
    /// </summary>
    public int 2P5M;

    /// <summary>
    /// 3P_3m이내
    /// </summary>
    public int 3P1M;

    /// <summary>
    /// 3P_5m이내
    /// </summary>
    public int 3P3M;

    /// <summary>
    /// 3P_7m이내
    /// </summary>
    public int 3P5M;

    /// <summary>
    /// 4P_3m이내
    /// </summary>
    public int 4P1M;

    /// <summary>
    /// 4P_5m이내
    /// </summary>
    public int 4P3M;

    /// <summary>
    /// 4P_7m이내
    /// </summary>
    public int 4P5M;

}
