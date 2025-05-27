using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 스탯, 상태정보
[Serializable]
public class PlayerStatData /*: NetworkBehaviour*/
{
    //public uint networkId;

    // 플레이어의 스탯
    public int id;             //플레이어 ID 정보(key)
    public string name;        //플레이어 이름 정보
    public int team;           //팀 정보
    public int maxHp;          //최대 체력(Health)
    public int curHp;          //현재 체력

    public int defGear;        // 방어구 체력
    public int def;            // 방어력

    public int speedMove;       // 이동속도

    public int startGold;       // 시작 골드
    public int startTeamCoin;   // 시작시 팀코인

    public int playerState;     // 플레이어의 상태: 100~112까지

    public int startWeapon1Id;          // 무기1 ID
    public int startAuxiliaryId;          // 보조무기 ID

    public int startConsumeItem1Id;          // 소비아이템1 ID            
}
