using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Wave_Run : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    Transform _target;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(_target.position);

            if (monster.AIPathing.remainingDistance <= 0.5f)
            {
                phase.ChangeState<DeadCop_Wave_HeadButt>();
            }
            else if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
            {
                phase.ChangeState<DeadCop_Wave_Punch>();
            }
            // wave ���������� �������� ���� ���ڸ� �� ������ IsWave�� false�� ���� ���̴�
            // FSM_DeadCop���� �ۼ�
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
