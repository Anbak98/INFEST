using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;

public class Monster_RageFang_FSM : MonsterFSM<Monster_RageFang>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!monster.targets.Contains(other.transform))
            {
                monster.targets.Add(other.transform);
                if(monster.target == null || monster.target.gameObject.layer != 7)
                {
                    monster.target = other.transform;
                }
            }
        }
    }
}
