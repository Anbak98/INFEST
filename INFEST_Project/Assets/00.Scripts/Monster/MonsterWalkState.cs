using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalkState : MonsterStateNetworkBehaviour
{
    public override void Enter()
    {
        base.Enter();
        if(HasStateAuthority)
        {
            monster.MovementSpeed = 1.5f;
        }
    }

    public override void Execute()
    {
        base.Execute();
        if(HasStateAuthority)
        {
            bool result = monster.AIPathing.SetDestination(monster.target.position);
        }
    }
}
