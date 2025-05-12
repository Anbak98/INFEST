using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave가 아니다
// Scream상태에서 할 일
// 소리 지르기
public class Grita_Scream : MonsterStateNetworkBehaviour<Monster_Grita>
{
    public override void Enter()
    {
        base.Enter();
        // Scream
        // 쿨타임, 횟수 제한
        if (monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
        {
            monster.Rpc_Scream();
            float animLength = monster.GetCurrentAnimLength();
            // Spawn
            monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
        }
    }
    public override void Execute()
    {
        base.Execute();
        // 추격 phase로 전환
        monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0번 상태 Run이 실행
    }

    public override void Exit()
    {
        base.Exit();
    }
}

