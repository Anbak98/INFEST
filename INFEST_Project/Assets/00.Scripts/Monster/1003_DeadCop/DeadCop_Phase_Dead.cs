using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Dead : MonsterPhase<Monster_DeadCop>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("DeadCop_Die");
        monster.CurMovementSpeed = 0;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
    }
}
