using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPhase : NetworkBehaviour
{
    [SerializeField]
    private List<MonsterStateNetworkBehaviour> states;
    private Dictionary<Type, MonsterStateNetworkBehaviour> stateMap = new();
    public MonsterStateNetworkBehaviour currentState;

    protected MonsterNetworkBehaviour monster;

    public void Init(MonsterNetworkBehaviour monster)
    {
        this.monster = monster;

        foreach (var state in states)
        {
            state.Init(monster, this);
            stateMap.Add(state.GetType(), state);
        }

        currentState = states[0];
    }

    public virtual void MachineExecute()
    {

    }

    public virtual void ExecuteState()
    {
        currentState.Execute();
    }

    public virtual void MachineEnter()
    {
        currentState.Enter();
    }

    public virtual void MachineExit()
    {
        currentState.Exit();
    }

    public void ChangeState<T>()
    {
        currentState?.Exit();
        currentState = stateMap[typeof(T)];
        currentState.Enter();
    }
}
