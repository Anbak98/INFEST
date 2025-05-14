using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Chase_Run : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
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

        // ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(_target.position);

            if (monster.AIPathing.remainingDistance <= 0.5f)
            {
                monster.IsAttack = true;
                phase.ChangeState<DeadCop_Chase_Attack_2_HeadButt>();
            }
            else if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
            {
                monster.IsAttack = true;
                phase.ChangeState<DeadCop_Chase_Attack_1_Normal>();
            }
            else
            {
                monster.FSM.ChangePhase<DeadCop_Phase_Wander>();
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
