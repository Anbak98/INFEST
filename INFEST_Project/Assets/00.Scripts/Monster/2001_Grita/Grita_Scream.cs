using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave가 아니다
// Scream상태에서 할 일
// 소리 지르기
public class Grita_Scream : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    public TickTimer _animTickTimer;

    public override void Enter()
    {
        base.Enter();
        // 
        if (!monster.screemCooldownTickTimer.Expired(Runner))
        {
            monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0번 상태 Run이 실행
            return;
        }

        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // 기술 썼으니
        monster.ScreamCount++;
        Debug.Log("Scream Enter");
        monster.Rpc_Scream();
        float animLength = monster.GetCurrentAnimLength();

        _animTickTimer = TickTimer.CreateFromSeconds(Runner, animLength);

        // Wave가 시작된다(무한생성되는 문제)
        //monster.spawner.SpawnMonsterOnWave(monster.target.transform);
    }
    public override void Execute()
    {
        base.Execute();
                
        if (_animTickTimer.Expired(Runner))     // _tickTimer가 해당 시간만큼 지나면 true가 된다
        {
            // 추격 phase로 전환
            monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0번 상태 Run이 실행
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Scream Exit");
        monster.IsScream = false;
    }
}

// Wave 단계(Wave phase와는 다르다)

