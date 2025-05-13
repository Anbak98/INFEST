using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave Phase의 첫번째 상태
// ScreamWave의 우선순위 > AttackWave의 우선순위
public class Grita_RunWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;

    }
    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // 앉으면 못알아채는건 나중에 구현해보자
        // 거리 15 이내일 때 Scream
        if (monster.AIPathing.remainingDistance <= 15f)
        {
            // Scream이 높은 우선순위, Attack이 낮은 우선순위
            phase.ChangeState<Grita_ScreamWave>();
            // 쿨타임 중이거나, 아니면 2번 실행했다면 Attack
            if (!monster.CanScream() || monster.screamCount >= Monster_Grita.screamMaxCount)
                phase.ChangeState<Grita_AttackWave>();
        }
        else if (!monster.IsLookPlayer() && monster.AIPathing.remainingDistance > 15f)
        {
            monster.target = null;
            monster.FSM.ChangePhase<Grita_Phase_Wander>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
