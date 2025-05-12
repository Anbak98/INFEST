using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (monster.IsLookPlayer())
        {
            ChangePhase<Grita_Phase_Chase>();
        }
    }
}
