using Fusion;
using UnityEngine;

public class BaseMonsterStateNetworkBehaviour : NetworkBehaviour
{
    public virtual void Init<T, F>(T monster, F phase) where T : BaseMonster<T> where F : MonsterPhase<T>
    {
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
