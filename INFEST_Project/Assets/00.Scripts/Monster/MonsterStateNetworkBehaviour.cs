using Fusion;
using UnityEngine;

/// <summary>
/// Init 
/// </summary>
public abstract class MonsterStateNetworkBehaviour : NetworkBehaviour
{
    /// <summary>
    /// Reference to Monster that control Phase that control this state
    /// </summary>
    protected MonsterNetworkBehaviour monster;

    /// <summary>
    /// Reference to Monster Phase that control this state
    /// </summary>
    protected MonsterPhase phase;

    public void Init(MonsterNetworkBehaviour monster, MonsterPhase phase)
    {
        this.monster = monster;
        this.phase = phase;
    }

    public virtual void Enter()
    {

    }   

    public virtual void Execute()
    {

    }

    public virtual void Exit() 
    { 
    }
}
