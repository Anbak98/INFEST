using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class BossRunPoint
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// KeyValue
    /// </summary>
    public string keyName;

    /// <summary>
    /// 이름
    /// </summary>
    public int Boss;

    /// <summary>
    /// 초기 X 좌표
    /// </summary>
    public int PointX;

    /// <summary>
    /// 초기 Y 좌표
    /// </summary>
    public int PointY;

    /// <summary>
    /// 초기 Z 좌표
    /// </summary>
    public float PointZ;

}
