using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_PJ_HI : MonsterNetworkBehaviour
{
    public override void PlayerDetectedListnerByPlayer()
    {
        base.PlayerDetectedListnerByPlayer();
        FSM.ChangePhase<PJ_HI_ChasePhase>();
    }
}
