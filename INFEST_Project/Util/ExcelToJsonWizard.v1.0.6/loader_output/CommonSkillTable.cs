using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CommonSkillTable
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
    /// 사용 우선순위
    /// </summary>
    public int SkillPriority;

}
