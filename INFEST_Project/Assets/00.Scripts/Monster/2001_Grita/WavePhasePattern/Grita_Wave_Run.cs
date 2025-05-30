using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave Phase�� ù��° ����
// ScreamWave�� �켱���� > AttackWave�� �켱����
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
