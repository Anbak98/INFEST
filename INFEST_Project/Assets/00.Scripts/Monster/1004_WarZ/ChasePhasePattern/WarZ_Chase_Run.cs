using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_Run : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();

        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
        if (monster.target != null)
           monster.MoveToTarget();

    }

    public override void Exit()
    {
        base.Exit();
        //monster.CurMovementSpeed = 0;
    }
}
