using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Chase_Attack_1_Normal : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
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

        float animTime = monster.GetCurrentAnimLength();
        monster.animTickTimer = TickTimer.CreateFromSeconds(Runner, animTime);
    }
    public override void Execute()
    {
        base.Execute();

        if (monster.animTickTimer.Expired(Runner))
        {
            monster.AIPathing.SetDestination(monster.target.position);
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
                {
                    phase.ChangeState<DeadCop_Chase_Attack_1_Normal>();
                }
                else
                {
                    monster.IsAttack = false;
                    phase.ChangeState<DeadCop_Chase_Run>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
