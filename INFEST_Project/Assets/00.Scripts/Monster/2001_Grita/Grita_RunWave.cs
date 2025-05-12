using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_RunWave : MonsterStateNetworkBehaviour<Monster_Grita>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;

    }
    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // 거리 15 이내일 때 Scream
        if (monster.AIPathing.remainingDistance <= 15f)
        {
            phase.ChangeState<Grita_ScreamWave>();
        }
        else 
        {
            monster.FSM.ChangePhase<Grita_Phase_Wonder>();
        }

    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
