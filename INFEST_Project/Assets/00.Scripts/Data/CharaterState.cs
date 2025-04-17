using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharaterState : IKeyedItem
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 상태
    /// </summary>
    public string State;

    /// <summary>
    /// 설명
    /// </summary>
    public string Discription;

    public int Key => key;

}
