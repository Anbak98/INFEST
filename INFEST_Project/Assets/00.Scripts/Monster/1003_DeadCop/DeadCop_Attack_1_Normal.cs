using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Attack_1_Normal : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsAttack = true;
        monster.CurMovementSpeed = 0f;

        //monster.targetStatHandler = monster.target.GetComponent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(10);
        monster.animTickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }
    public override void Execute()
    {
        base.Execute();

        if (monster.animTickTimer.Expired(Runner))
        {
            phase.ChangeState<DeadCop_Idle>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsAttack = false;
    }
}
