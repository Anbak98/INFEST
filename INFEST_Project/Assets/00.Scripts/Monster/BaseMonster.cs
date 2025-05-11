using System;
using UnityEngine;

public class BaseMonster<T> : MonsterNetworkBehaviour where T : BaseMonster<T>
{
    [Tooltip("Reference to the enemy's FSM.")]
    public MonsterFSM<T> FSM;
}
