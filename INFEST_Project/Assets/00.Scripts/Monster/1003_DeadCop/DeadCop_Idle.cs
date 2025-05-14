using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Idle : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wander>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Bowmeter_Walk>();
        }
    }
}
