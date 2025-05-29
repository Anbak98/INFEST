using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_FlexingMuscles : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsFlexingMuscles = true;
        phase.skillCoolDown[4] = TickTimer.CreateFromSeconds(Runner, monster.skills[4].CoolDown);
        monster.Buff(30, (int)(monster.BaseDamage / monster.skills[4].DamageCoefficient), (int)(monster.BaseDef / monster.skills[4].DamageCoefficient));
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        monster.IsFlexingMuscles = false;
        base.Exit();
    }
}
