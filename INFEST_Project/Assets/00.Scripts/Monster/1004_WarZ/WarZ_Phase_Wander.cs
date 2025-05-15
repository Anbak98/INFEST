using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Wander : MonsterPhase<Monster_WarZ>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.PhaseIndex = 0;
        monster.animator.Play("Wander.WarZ_Idle");
        Debug.Log("Wander");
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
    }
}
