using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ConsumeItem : IKeyedItem
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
    /// 효과
    /// </summary>
    public int Effect;

    /// <summary>
    /// 최대 보유 가능 개수
    /// </summary>
    public int MaxNum;

    /// <summary>
    /// 아이템 타입
    /// </summary>
    public DesignEnums.ConsumeItemType ConsumeItemType;

    /// <summary>
    /// 아이템 가격
    /// </summary>
    public int Price;

    public int Key => key;

}
