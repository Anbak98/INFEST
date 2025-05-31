using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Wander : MonsterPhase<Monster_DeadCop>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.PhaseIndex = 0;
        monster.animator.Play("Wander.DeadCop_Idle");
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsFindPlayer() && !monster.IsDead)
        {
            monster.FSM.ChangePhase<DeadCop_Phase_Chase>();
        }
    }
}
