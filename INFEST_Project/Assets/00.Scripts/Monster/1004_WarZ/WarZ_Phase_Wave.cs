using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Wave : MonsterPhase<Monster_WarZ>
{
    public override void MachineEnter()
    {
        base.MachineEnter();

        monster.animator.Play("Wave.WarZ_Run");
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.PlayerDetectorCollider.radius != monster.info.DetectAreaWave)
            monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
    }

    public override void MachineExit()
    {
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaNormal;
        base.MachineExit();
    }
}
