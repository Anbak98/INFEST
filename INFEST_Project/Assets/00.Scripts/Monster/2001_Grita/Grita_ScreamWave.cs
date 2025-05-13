using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave상태에서 할 일
// 소리 지르기
// 몬스터 스폰하기

public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita>
{

    public override void Enter()
    {
        base.Enter();
        // Scream
        if (monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
        {
            monster.Rpc_Scream();   // 여기서 모두 해버리면.. 상태머신에서 할 것이 없다
            float animLength = monster.GetCurrentAnimLength();
            // Spawn
            monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
        }
    }
    public override void Execute()
    {
        // 코루틴 돌때 여기 실행되는지 확인
        Debug.Log("Execute 실행");
        base.Execute();

        // Enter에서 공격했으니 RunWave로 상태전환
        phase.ChangeState<Grita_RunWave>();

    }

    public override void Exit()
    {
        base.Exit();

    }

}

// 추가 스폰
// MonsterSpawner가 알아서 해주니까 몬스터 생성되는건 RPC가 필요하지 않다
// SpawnMonsterOnWave에서 내부적으로 Runner.Spawn으로 생성
// host에서 1번만 호출하면 된다

// 몬스터를 7~10 추가 스폰
// 몬스터별로 스폰 확률이 다르니까 기획서를 참고
