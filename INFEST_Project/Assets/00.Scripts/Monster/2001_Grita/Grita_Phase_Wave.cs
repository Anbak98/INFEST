using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wave : MonsterPhase<Monster_Grita>
{
    public override void MachineEnter()
    {
        base.MachineEnter();


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
