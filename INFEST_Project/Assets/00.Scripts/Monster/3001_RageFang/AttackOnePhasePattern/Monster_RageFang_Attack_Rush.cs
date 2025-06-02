using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_Rush : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySfx(Sfxs.RageFang_Rush);
        monster.CurMovementSpeed = 15;
        monster.IsRush = true;
        phase.skillCoolDown[5] = TickTimer.CreateFromSeconds(Runner, monster.skills[5].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Execute()
    {
        base.Execute();
        if(monster.IsTargetInRange(5f))
        {
            monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[5].DamageCoefficient));
            monster.CurMovementSpeed = 0;
            monster.IsReadyForChangingState = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
        monster.IsRush = false;
    }
}
