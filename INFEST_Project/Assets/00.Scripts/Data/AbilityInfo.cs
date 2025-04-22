using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AbilityInfo : IKeyedItem
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
    /// 적용 수치
    /// </summary>
    public int Effect;

    /// <summary>
    /// 어빌리티 타입
    /// </summary>
    public DesignEnums.AbilityType AbilityType;

    /// <summary>
    /// 최대 구매 횟수 제한
    /// </summary>
    public int MaxCount;

    /// <summary>
    /// 가격
    /// </summary>
    public int Price;
    public int Key => key;

}
