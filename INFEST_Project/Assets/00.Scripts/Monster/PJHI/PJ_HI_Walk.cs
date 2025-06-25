using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PJ_HI_Walk : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Wonder>
{
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
            if (monster.MoveToRandomPositionAndCheck(5f, 10f, 10f))
            {
                    phase.ChangeState<PJ_HI_Idle>();                
            }
        }
    }
}
