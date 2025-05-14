using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Wander_Idle : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wander>
{
    [SerializeField]
    private float DurationTime = 7.0f;
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, DurationTime);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<DeadCop_Walk>();
        }
    }
}
