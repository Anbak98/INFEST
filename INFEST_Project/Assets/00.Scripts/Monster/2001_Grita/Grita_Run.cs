using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Run : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Chase>
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

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(_target.position);

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<Grita_Attack>();
            }
            else if (!monster.IsLookPlayer() && monster.AIPathing.remainingDistance > 10f)
            {
                monster.target = null;
                monster.FSM.ChangePhase<Grita_Phase_Wander>();
            }
        }


    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
