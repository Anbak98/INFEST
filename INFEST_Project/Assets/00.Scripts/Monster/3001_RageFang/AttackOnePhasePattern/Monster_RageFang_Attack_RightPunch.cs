using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_RightPunch : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackOne>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsRightPunch = true;
        phase.patternTickTimer = TickTimer.CreateFromSeconds(Runner, 2);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRightPunch = false;
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget(monster.CurDamage);
    }
}
