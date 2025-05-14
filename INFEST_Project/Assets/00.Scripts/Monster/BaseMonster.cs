using System;
using UnityEngine;

public class BaseMonster<T> : MonsterNetworkBehaviour where T : BaseMonster<T>
{
    [Tooltip("Reference to the enemy's FSM.")]
    public MonsterFSM<T> FSM;

    public bool IsReadyForChangingState = true;

    public void OnAnimationEventIsReady()
    {
        IsReadyForChangingState = true;
    }

    public void OnAnimationEventIsAttack()
    {
        FSM.currentPhase.currentState.Attack();
    }

    public void OnAnimationEventIsEffect()
    {
        FSM.currentPhase.currentState.Effect();
    }
}
