using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Dead : MonsterPhase<Monster_Grita>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsDead = true;
        monster.animator.Play("Grita_Dead");
        monster.CurMovementSpeed = 0;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
    }

}
