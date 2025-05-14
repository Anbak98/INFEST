using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Run : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    Transform _target;

    public override void Enter()
    {
        base.Enter();
        _target = monster.target;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(_target.position);

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<DeadCop_Attack_1_Normal>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
