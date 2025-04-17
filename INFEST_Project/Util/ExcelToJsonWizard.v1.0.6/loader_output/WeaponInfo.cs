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
    /// 무기 종류
    /// </summary>
    public DesignEnums.EWeaponType WeaponType;

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
    public int MaxMagzine;

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

    /// <summary>
    /// 사격 방식 (T: 자동, F: 단발)
    /// </summary>
    public bool IsAutpmatic;

    /// <summary>
    /// 총알 속도
    /// </summary>
    public float FireRate;

    /// <summary>
    /// 사격당 나가는 탄환 개수
    /// </summary>
    public int ProjectilesPerShot;

    /// <summary>
    /// 총 가격
    /// </summary>
    public int Price;

    /// <summary>
    /// 총알 개당 가격
    /// </summary>
    public int BulletPrice;

}
