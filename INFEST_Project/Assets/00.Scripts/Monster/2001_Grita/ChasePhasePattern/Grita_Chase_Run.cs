using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Chase_Run : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Chase>
{

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
