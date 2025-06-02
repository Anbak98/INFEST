using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadCop_Wander_Walk : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wander>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();// 아직 경로가 계산되지 않았거나 도착한 경우
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5, 10, 10))
            {
                phase.ChangeState<DeadCop_Wander_Idle>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
