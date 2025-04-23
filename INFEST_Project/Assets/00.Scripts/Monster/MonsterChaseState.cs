using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : MonsterStateNetworkBehaviour
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 10f;
    }

    public override void Execute()
    {
        base.Execute();
        if (HasStateAuthority)
        {
            bool result = monster.AIPathing.SetDestination(monster.target.position);
        }
    }
}
