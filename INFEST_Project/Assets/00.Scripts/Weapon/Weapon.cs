using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


public class Weapon : NetworkBehaviour
{
    [Header("Basic"), SerializeField]
    public string name; // 공격력
    public float damage; // 공격력
    public bool isAutomatic; // 연사 or 단발
    public float atkRange; // 공격 사거리
    public float atkSpeed; // 공격 속도
    public float atkAccuracy; // 공격 정확도
    public int maxBullet; // 최대 탄약
    public int price; // 가격



}
