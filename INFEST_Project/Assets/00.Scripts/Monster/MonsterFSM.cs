using Fusion;
using UnityEngine;
using System;
using System.Collections.Generic;

public class MonsterFSM : NetworkBehaviour
{
    public MonsterNetworkBehaviour monster;
    [Header("Phase")]
    public MonsterPhase currentPhase;

    [SerializeField]
    private List<MonsterPhase> phases;

    private Dictionary<Type, MonsterPhase> phaseMap = new();

    public override void Spawned()
    {
        foreach (MonsterPhase m in phases)
        {
            m.Init(monster);
            phaseMap.Add(m.GetType(), m);
        }

        currentPhase.MachineEnter();
    }

    public override void FixedUpdateNetwork()
    {
        currentPhase.ExecuteState();
    }

    public void ChangePhase<T>() where T : MonsterPhase
    {
        if(currentPhase == phaseMap[typeof(T)])
        {
            return;
        }
        currentPhase?.MachineExit();
        currentPhase = phaseMap[typeof(T)];
        currentPhase.MachineEnter();
    }

    public void ChangeState<T>() where T : MonsterStateNetworkBehaviour
    {
        currentPhase.ChangeState<T>();
    }
}
