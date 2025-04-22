using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_PJ_HI : MonsterNetworkBehaviour
{
    public LayerMask targetLayer;

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log(other.gameObject.layer);
            target = other.gameObject.transform;
            FSM.ChangePhase<PJ_HI_ChasePhase>();
        } 
    }
}
