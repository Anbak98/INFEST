using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJ_HI_Attack : MonsterStateNetworkBehaviour
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        monster.IsAttack = true;
        monster.targetStatHandler.TakeDamage(10);
        Invoke(nameof(OnEndAttack), 2f);
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);
        if (!monster.AIPathing.pathPending)
        {
            if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance && !monster.IsAttack)
            {
                phase.ChangeState<PJ_HI_Run>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit(); 
    }

    private void OnEndAttack()
    {
        monster.IsAttack = false;
        if (!monster.AIPathing.pathPending)
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
