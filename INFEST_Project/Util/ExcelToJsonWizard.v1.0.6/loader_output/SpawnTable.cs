using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SpawnTable
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
    /// 웨이브 초기 스폰 비율
    /// </summary>
    public int StartByWave;

    /// <summary>
    /// 10분당 스폰 웨이브 비율 증감량
    /// </summary>
    public int WavePer10Min;

    /// <summary>
    /// 필드 초기 스폰 확률
    /// </summary>
    public float StartByField;

    /// <summary>
    /// 5분당 필드스폰 확률 증감량
    /// </summary>
    public float FieldPer5Min;

    /// <summary>
    /// 스크림 의한 초기 스폰확률
    /// </summary>
    public float StartByScream;

    /// <summary>
    /// 5분당 스폰 확률 증감량
    /// </summary>
    public float ScreamPer5Min;

}
