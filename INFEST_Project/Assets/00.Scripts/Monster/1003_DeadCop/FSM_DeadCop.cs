using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_DeadCop : MonsterFSM<Monster_DeadCop>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        // 모든 phase와 state에 적용된다

        // 웨이브 진입(bool값으로 조절할 것 같다)
        //if (웨이브 시작때 bool값을 true로 한다든가, 아니면 웨이브로 인해 생성된 좀비 숫자가 숫자가 0이 아니라면)
        //{
        //    // Wave 상태로 들어간다
        //    monster.FSM.ChangePhase<DeadCop_Phase_Wave>();
        //}

        // 웨이브가 끝났다면
        //if (웨이브 시작때 생성된 마리수를 모두 잡으면)
        //{
        //    // Wander 상태로 돌아간다
        //    monster.FSM.ChangePhase<DeadCop_Phase_Wander>();
        //}

        if (monster.IsLookPlayer())
        {
            ChangePhase<DeadCop_Phase_Chase>();
        }
    }
}
