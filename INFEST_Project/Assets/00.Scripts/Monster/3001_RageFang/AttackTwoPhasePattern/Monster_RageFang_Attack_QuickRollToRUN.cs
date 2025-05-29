using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_QuickRollToRUN : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 8;
        monster.IsQuickRollToRun = true;
        phase.skillCoolDown[8] = TickTimer.CreateFromSeconds(Runner, monster.skills[8].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
        monster.IsQuickRollToRun = false;
    }
}
