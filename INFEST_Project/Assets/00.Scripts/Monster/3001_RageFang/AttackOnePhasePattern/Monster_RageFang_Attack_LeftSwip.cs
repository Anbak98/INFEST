using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_LeftSwip : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsLeftSwip = true;
        phase.skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, monster.skills[2].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsLeftSwip = false;
    }

    public override void Attack()
    {
        base.Attack();

        var state = monster.GetTryTargetState(monster.target);
        if (state is PlayerWaddleState || state is PlayerSitIdleState || state is PlayerSitState)
        {
            Debug.Log("Dodge");
            return;
        }

        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[2].DamageCoefficient));
    }
}
