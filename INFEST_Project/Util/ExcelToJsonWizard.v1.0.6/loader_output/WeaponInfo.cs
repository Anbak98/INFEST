using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class WeaponInfo
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
    /// 공격력
    /// </summary>
    public int Atk;

    /// <summary>
    /// 사정거리
    /// </summary>
    public int WeaponRange;

    /// <summary>
    /// 탄창 총알 개수
    /// </summary>
    public int MagazineBullet;

    /// <summary>
    /// 최대 총알 개수
    /// </summary>
    public int MaxBullet;

    /// <summary>
    /// 반동 힘
    /// </summary>
    public float RecoilForce;

    /// <summary>
    /// 반동복귀시간
    /// </summary>
    public float RecoilReturnTime;

    /// <summary>
    /// 집탄율
    /// </summary>
    public float Concentration;

    /// <summary>
    /// 광역 데미지
    /// </summary>
    public float Splash;

}
