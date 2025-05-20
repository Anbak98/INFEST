using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_DeadCop : MonsterFSM<Monster_DeadCop>
{
    public LayerMask targetLayerMask;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if ((targetLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
        }
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == targetLayerMask.value)
        {
            monster.TryRemoveTarget(other.transform);
        }
    }
}
