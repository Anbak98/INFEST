using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Scream : MonsterPhase<Monster_Grita>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsReadyForChangingState = false;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        if(monster.IsReadyForChangingState)
        {
            monster.FSM.ChangePhase<Grita_Phase_Chase>();
        }
    }
}
