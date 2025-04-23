using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : NetworkBehaviour
{
    [SerializeField]
    private List<MonsterStateNetworkBehaviour> states;
    private Dictionary<Type, MonsterStateNetworkBehaviour> stateMap = new();
    public MonsterStateNetworkBehaviour currentState;

    public override void Spawned()
    {
        foreach(var state in states)
        {
            stateMap.Add(state.GetType(), state);
        }
    }

    public void MachineEnter()
    {

    }

    public void MachineExit()
    {
    }

    public void ChangeState<T>()
    {
        currentState?.Exit();
        currentState = stateMap[typeof(T)];
        currentState.Enter();
    }
}
