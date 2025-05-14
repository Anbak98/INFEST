using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<Grita_Phase_Chase>();
        }
    }
}
