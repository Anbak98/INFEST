using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave상태에서 할 일
// 소리 지르기
// 몬스터 스폰하기

/// <summary>
/// 웨이브를 잘못 이해해서 바꾸는 중이다
/// 이건 임시로 남겼다
///
/// </summary>
public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // 기술 썼으니
        monster.ScreamCount++;

        Debug.Log("Scream Wave Enter");

        monster.Rpc_Scream();
    }

    public override void Execute()
    {
        // 코루틴 돌때 여기 실행되는지 확인
        Debug.Log("Execute 실행");
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
        Debug.Log("Scream Wave Exit");
    }
}

// Wave phase
// MonsterSpawner가 알아서 해주니까 몬스터 생성되는건 RPC가 필요하지 않다
// SpawnMonsterOnWave에서 내부적으로 Runner.Spawn으로 생성
// host에서 1번만 호출하면 된다
// 몬스터를 7~10 추가 스폰
// 몬스터별로 스폰 확률이 다르니까 기획서를 참고
