using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// WonderPhase에만 포함된 상태
public class Grita_Wander_Walk : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();
        base.Execute();// 아직 경로가 계산되지 않았거나 도착한 경우
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5f, 10f, 10f))
            {
                phase.ChangeState<Grita_Wander_Idle>();
            }
        }
    }
}
