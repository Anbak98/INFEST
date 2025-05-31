using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Chase_Run : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        //_target = monster.target;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        //monster.CurMovementSpeed = 0;
    }
}
