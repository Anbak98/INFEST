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
            // 소리 한번 지르는게 먼저다
            // 그러면 ScreamPhase를 따로 만들어야하나? 이미 State와 Wave로 만들었는데?
        }
    }
}
