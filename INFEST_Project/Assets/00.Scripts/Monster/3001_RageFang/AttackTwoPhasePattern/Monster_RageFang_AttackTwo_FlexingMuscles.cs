using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_FlexingMuscles : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsFlexingMuscles = true;
        phase.skillCoolDown[4] = TickTimer.CreateFromSeconds(Runner, monster.skills[4].CoolDown);
        //phase.flexingMusclesBuffTimer = TickTimer.CreateFromSeconds(Runner, 30);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsFlexingMuscles = false;
    }
}
