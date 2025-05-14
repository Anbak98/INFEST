using Fusion;
using UnityEngine;

/// <summary>
/// Init 
/// </summary>
public abstract class MonsterStateNetworkBehaviour<T, F> : BaseMonsterStateNetworkBehaviour where T : BaseMonster<T> where F : MonsterPhase<T>
{
    /// <summary>
    /// Reference to Monster that control Phase that control this state
    /// </summary>
    protected T monster;

    /// <summary>
    /// Reference to Monster Phase that control this state
    /// </summary>
    protected F phase;

    public override void Init<S, G>(S monster, G phase)
    {
        this.monster = monster as T;
        this.phase = phase as F;
    }    
}
