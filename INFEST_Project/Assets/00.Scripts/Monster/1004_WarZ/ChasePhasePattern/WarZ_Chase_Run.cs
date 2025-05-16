using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_Run : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;

        Debug.Log(monster.target.transform.position);
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);

    }

    public override void Exit()
    {
        base.Exit();
        //monster.CurMovementSpeed = 0;
    }
}
