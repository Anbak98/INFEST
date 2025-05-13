using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPhase<T> : NetworkBehaviour where T : BaseMonster<T> 
{
    [SerializeField]
    private List<BaseMonsterStateNetworkBehaviour> states;
    private Dictionary<Type, BaseMonsterStateNetworkBehaviour> stateMap = new();
    [ReadOnly] public BaseMonsterStateNetworkBehaviour currentState;

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

    public void ChangeState<S>()
    {
        currentState?.Exit();
        currentState = stateMap[typeof(S)];
        currentState.Enter();
    }
}
