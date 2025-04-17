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

    private void OnEndAttack()
    {
        phase.ChangeState<PJ_HI_Run>();
    }

    public override void Exit()
    {
        monster.IsAttack = false;
        base.Exit();
    }
}
