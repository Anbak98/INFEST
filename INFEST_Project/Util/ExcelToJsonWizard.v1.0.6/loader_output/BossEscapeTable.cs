using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class BossEscapeTable
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
    /// 인원수 증가계수
    /// </summary>
    public float IncreaseCoef;

    /// <summary>
    /// 5분 이내 도망치는 피
    /// </summary>
    public int Escape0Min;

    /// <summary>
    /// 10분 이내 도망치는 피
    /// </summary>
    public int Escape5Min;

    /// <summary>
    /// 15분 이내 도망치는 피
    /// </summary>
    public int Escape10Min;

    /// <summary>
    /// 20분 이내 도망치는 피
    /// </summary>
    public int Escape15Min;

    /// <summary>
    /// 20분 이내 도망치는 피
    /// </summary>
    public int Escape20Min;

    /// <summary>
    /// 25분 이후 도망치는 피
    /// </summary>
    public int Escape25Min;

}
