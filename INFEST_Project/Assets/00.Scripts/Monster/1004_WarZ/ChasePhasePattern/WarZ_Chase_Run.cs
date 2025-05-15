using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_Run : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    Transform _target;

    public override void Enter()
    {
        base.Enter();
        //_target = monster.target;
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);

        monster.AIPathing.SetDestination(_target.position);
        //// 아직 경로가 계산되지 않았거나 도착한 경우
        //if (/*monster.AIPathing.enabled && */!monster.AIPathing.pathPending)
        //{

        //    //if (monster.AIPathing.remainingDistance <= 0.5f)
        //    //{
        //    //    monster.IsAttack = true;
        //    //    phase.ChangeState<WarZ_Chase_DropKick>();
        //    //}
        //    //else if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
        //    //{
        //    //    monster.IsAttack = true;
        //    //    phase.ChangeState<WarZ_Chase_Punch>();
        //    //}
        //    //else
        //    //{
        //    //    monster.FSM.ChangePhase<WarZ_Phase_Wander>();
        //    //}

        //    //else if (monster.AIPathing.remainingDistance > 10f)
        //    //{
        //    //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
        //    //}

        //}
    }

    public override void Exit()
    {
        base.Exit();
        //monster.CurMovementSpeed = 0;
    }
}
