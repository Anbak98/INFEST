using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPhase<T> : NetworkBehaviour where T : BaseMonster<T>
{
    [SerializeField]
    private List<MonsterStateNetworkBehaviour<T>> states;
    private Dictionary<Type, MonsterStateNetworkBehaviour<T>> stateMap = new();
    [ReadOnly] public MonsterStateNetworkBehaviour<T> currentState;

    protected T monster;

    public void Init(T monster)
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
