using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_DeadCop : MonsterFSM<Monster_DeadCop>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<DeadCop_Phase_Chase>();
        }
    }
}
