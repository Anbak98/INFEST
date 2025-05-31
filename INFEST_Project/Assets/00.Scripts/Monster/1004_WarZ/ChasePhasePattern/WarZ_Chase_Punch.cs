using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_Punch : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        monster.IsRightPunch = true;

        phase.CoolDown[1] = TickTimer.CreateFromSeconds(Runner, monster.CommonSkillTable[1].CoolDown);
        // 애니메이션이 끝나기 전에는 상태가 안바뀐다
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
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }
}
