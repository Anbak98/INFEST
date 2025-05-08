using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Run : MonsterStateNetworkBehaviour
{
    Transform _target;

    public override void Enter()
    {
        base.Enter();
        _target = monster.target;
        monster.MovementSpeed = monster.info.SpeedMoveWave;
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
                phase.ChangeState<Grita_Attack>();
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
