using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Chase_Run : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Chase>
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

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<Grita_Chase_Attack>();
            }
            else if (!monster.IsLookPlayer() && monster.AIPathing.remainingDistance > 10f)
            {
                monster.TryRemoveTarget(monster.target);
                monster.FSM.ChangePhase<Grita_Phase_Wander>();
            }
        }


    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
