using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MapData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 맵 이름
    /// </summary>
    public string MapName;

    /// <summary>
    /// 등장하는 보스
    /// </summary>
    public int BossZombie;

}
