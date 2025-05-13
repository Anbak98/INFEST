using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave상태에서 할 일
// 소리 지르기
// 몬스터 스폰하기

public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // 기술 썼으니
        monster.ScreamCount++;

        Debug.Log("Scream Wave Enter");

        monster.Rpc_Scream();
        float animLength = monster.GetCurrentAnimLength();

        _tickTimer = TickTimer.CreateFromSeconds(Runner, animLength);   // 해당 시간이 지난 다음 다음 진행

        // Spawn
        //monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
    }
    public override void Execute()
    {
        // 코루틴 돌때 여기 실행되는지 확인
        Debug.Log("Execute 실행");
        base.Execute();

        // Enter에서 공격했으니 RunWave로 상태전환
        if (_tickTimer.Expired(Runner))     // _tickTimer가 해당 시간만큼 지나면 true가 된다
        {
            phase.ChangeState<Grita_RunWave>();
        }

    }

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
        Debug.Log("Scream Wave Exit");
    }
}

// 추가 스폰
// MonsterSpawner가 알아서 해주니까 몬스터 생성되는건 RPC가 필요하지 않다
// SpawnMonsterOnWave에서 내부적으로 Runner.Spawn으로 생성
// host에서 1번만 호출하면 된다

// 몬스터를 7~10 추가 스폰
// 몬스터별로 스폰 확률이 다르니까 기획서를 참고
