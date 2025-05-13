using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJ_HI_Attack : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.MovementSpeed = 0f;
        monster.IsAttack = true;

        //monster.targetStatHandler = monster.target.GetComponentInParent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(Random.Range(monster.info.MinAtk, monster.info.MaxAtk));
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void Execute()
    {
        base.Execute(); 
        if (_tickTimer.Expired(Runner))
        {
            monster.AIPathing.SetDestination(monster.target.position);
            monster.IsAttack = false;
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<PJ_HI_Attack>();
                }
                else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<PJ_HI_Run>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit(); 
    }
}
