using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Wander_Idle : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Wander>
{
    [SerializeField]
    private float DurationTime = 2.0f;
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        Debug.Log("Idle");

        _tickTimer = TickTimer.CreateFromSeconds(Runner, DurationTime);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<WarZ_Wander_Walk>();
        }
    }
}
