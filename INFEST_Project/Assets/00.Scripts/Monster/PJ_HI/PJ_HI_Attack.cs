using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJ_HI_Attack : MonsterStateNetworkBehaviour
{
    public override void Enter()
    {
        base.Enter();
        monster.IsAttack = true;
        Invoke(nameof(OnEndAttack), 0.2f);
    }
    

    public override void Exit()
    {
        monster.IsAttack = false;
        base.Exit();
    }
    private void OnEndAttack()
    {
        if (!monster.AIPathing.pathPending)
        {
            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<PJ_HI_Attack>();
            }
            else if (monster.AIPathing.remainingDistance > 10f)
            {
                monster.FSM.ChangePhase<PJ_HI_ChasePhase>();
            }
        }
    }
}
