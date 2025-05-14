using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_DeadCop : MonsterFSM<Monster_DeadCop>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (monster.IsLookPlayer())
        {
            ChangePhase<Bowmeter_Phase_Chase>();
        }
    }
}
