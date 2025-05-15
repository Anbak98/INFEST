using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Dead : MonsterPhase<Monster_WarZ>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("WarZ_Dead");
        monster.CurMovementSpeed = 0;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
    }
}
