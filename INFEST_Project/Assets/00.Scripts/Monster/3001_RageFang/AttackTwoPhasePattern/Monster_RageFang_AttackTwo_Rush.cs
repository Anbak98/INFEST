using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_Rush : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 30;
        monster.IsRush = true;
        phase.skillCoolDown[5] = TickTimer.CreateFromSeconds(Runner, monster.skills[5].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Execute()
    {
        base.Execute();
        if(monster.AIPathing.remainingDistance < 5f)
        {
            monster.TryAttackTarget((int)(monster.CurDamage*1.3));
            monster.CurMovementSpeed = 0;
            monster.IsReadyForChangingState = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRush = false;
    }
}
