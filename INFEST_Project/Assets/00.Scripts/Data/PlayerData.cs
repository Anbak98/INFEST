using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData : IKeyedItem
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// index
    /// </summary>
    public string Playeridx;

    /// <summary>
    /// 닉네임
    /// </summary>
    public string NickName;

    public int Key => key;

}
