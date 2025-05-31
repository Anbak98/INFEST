using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_ContinuousPunch : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    private int punchCount;
    public override void Enter()
    {
        base.Enter();
        punchCount = 0;
        monster.CurMovementSpeed = 0;
        monster.IsContinousAttack = true;
        phase.skillCoolDown[7] = TickTimer.CreateFromSeconds(Runner, monster.skills[7].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsContinousAttack = false;
    }

    public override void Attack()
    {
        base.Attack();
        punchCount++;
        if(punchCount == 3)
        {
            if(monster.GetTryTargetState(monster.target) is PlayerWaddleState)
            {
                return;
            }
        }

        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[7].DamageCoefficient));
    }
}
