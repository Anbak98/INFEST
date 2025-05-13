using Fusion;
using UnityEngine;
using System;
using System.Collections.Generic;

public class MonsterFSM<T> : NetworkBehaviour where T : BaseMonster<T>
{
    public T monster;
    [Header("Phase")]
    public MonsterPhase<T> currentPhase;

    [SerializeField]
    private List<MonsterPhase<T>> phases;

    private Dictionary<Type, MonsterPhase<T>> phaseMap = new();

    public override void Spawned()
    {
        foreach (MonsterPhase<T> m in phases)
        {
            m.Init(monster);
            phaseMap.Add(m.GetType(), m);
        }

        currentPhase.MachineEnter();
    }

    public override void FixedUpdateNetwork()
    {
        currentPhase.MachineExecute();
    }

    public void ChangePhase<S>()
    {
        if(currentPhase == phaseMap[typeof(S)])
        {
            return;
        }
        currentPhase?.MachineExit();
        currentPhase = phaseMap[typeof(S)];
        currentPhase.MachineEnter();
    }

    public void ChangeState<S>()
    {
        currentPhase.ChangeState<S>();
    }
}
