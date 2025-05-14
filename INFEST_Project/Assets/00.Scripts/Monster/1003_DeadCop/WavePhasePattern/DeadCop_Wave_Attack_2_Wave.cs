using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Wave_Attack_2_Wave : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.CurMovementSpeed = 0f;
        monster.IsAttack = true;

        //monster.targetStatHandler = monster.target.GetComponentInParent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(Random.Range(monster.info.MinAtk, monster.info.MaxAtk));

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
                if (monster.AIPathing.remainingDistance <= 0.5f)
                {
                    phase.ChangeState<DeadCop_Wave_Attack_2_Wave>();
                }
                else
                {
                    monster.IsAttack = false;
                    phase.ChangeState<DeadCop_Wave_RunWave>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
