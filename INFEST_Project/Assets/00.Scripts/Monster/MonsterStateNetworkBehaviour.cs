using Fusion;
using UnityEngine;

/// <summary>
/// Init 
/// </summary>
public abstract class MonsterStateNetworkBehaviour<T> : NetworkBehaviour where T : BaseMonster<T>
{
    /// <summary>
    /// Reference to Monster that control Phase that control this state
    /// </summary>
    protected T monster;

    /// <summary>
    /// Reference to Monster Phase that control this state
    /// </summary>
    protected MonsterPhase<T> phase;

    public void Init(T monster, MonsterPhase<T> phase)
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
