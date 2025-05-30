using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Wave_Run : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

       monster.MoveToTarget();
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
