using Fusion;
using UnityEngine;
using System;
using System.Collections.Generic;

public class MonsterFSM : NetworkBehaviour
{
    public Monster monster;

    public MonsterStateMachine currentPhase;

    [SerializeField]
    private List<MonsterStateMachine> phases;

    private Dictionary<Type, MonsterStateMachine> phaseMap = new();

    public override void Spawned()
    {
        foreach (MonsterStateMachine m in phases)
        {
            phaseMap.Add(m.GetType(), m);
        }

        currentPhase.currentState.Enter();
    }

    public override void FixedUpdateNetwork()
    {
        currentPhase.currentState.Execute();
    }

    public void ChangePhase<T>() where T : MonsterStateMachine
    {
        currentPhase?.MachineExit();
        currentPhase = phaseMap[typeof(T)];
        currentPhase.MachineEnter();
    }

    public void ChangeState<T>() where T : MonsterStateNetworkBehaviour
    {
        currentPhase.ChangeState<T>();
    }
}
