using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MysteryBoxTable
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
    /// 이지모드 등장확률
    /// </summary>
    public int EasyProbability;

    /// <summary>
    /// 노말모드 등장확률
    /// </summary>
    public int NormalProbability;

    /// <summary>
    /// 하드모드 등장확률
    /// </summary>
    public int HardProbability;

    /// <summary>
    /// 헬모드 등장확률
    /// </summary>
    public int HellProbability;

}
