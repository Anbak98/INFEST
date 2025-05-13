using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_LeftSwip : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackOne>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0;
        monster.IsLeftSwip = true;
        phase.skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, monster.skills[2].CoolDown);
        phase.patternTickTimer = TickTimer.CreateFromSeconds(Runner, 3);
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsLeftSwip = false;
    }
}
