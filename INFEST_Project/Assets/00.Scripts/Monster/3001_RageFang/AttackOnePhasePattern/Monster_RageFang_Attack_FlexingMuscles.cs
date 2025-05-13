using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_FlexingMuscles : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackOne>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsFlexingMuscles = true;
        phase.skillCoolDown[4] = TickTimer.CreateFromSeconds(Runner, monster.skills[4].CoolDown);
        phase.patternTickTimer = TickTimer.CreateFromSeconds(Runner, 3);
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsFlexingMuscles = false;
    }
}
