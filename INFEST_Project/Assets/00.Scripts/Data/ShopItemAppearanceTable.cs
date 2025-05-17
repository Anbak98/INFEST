using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ShopItemAppearanceTable : IKeyedItem
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
    /// 쉬움모드 존재 개수
    /// </summary>
    public int EasyMode;

    /// <summary>
    /// 노말모드 존재 개수
    /// </summary>
    public int NormalMode;

    /// <summary>
    /// 하드모드 존재 개수
    /// </summary>
    public int HardMode;

    /// <summary>
    /// 헬모드 존재 개수
    /// </summary>
    public int HellMode;
    public int Key => key;
}
