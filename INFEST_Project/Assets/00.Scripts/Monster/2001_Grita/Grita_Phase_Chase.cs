using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Chase : MonsterPhase<Monster_Grita>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.AIPathing.speed = monster.info.SpeedMoveWave;

        monster.IsReadyForChangingState = true;
        monster.IsChasePhase = true;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= monster.skill[1].UseRange)
                {
                    ChangeState<Grita_Chase_Attack>();
                }
                else
                {
                    ChangeState<Grita_Chase_Run>();
                }
            }
        }
    }
}
