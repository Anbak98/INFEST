using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave Phase의 첫번째 상태
// ScreamWave의 우선순위 > AttackWave의 우선순위
public class Grita_Wave_Run : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
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
