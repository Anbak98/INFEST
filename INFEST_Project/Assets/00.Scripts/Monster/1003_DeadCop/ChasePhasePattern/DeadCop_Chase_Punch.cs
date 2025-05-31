using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Chase_Punch : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        monster.IsRightPunch = true;
        phase.CoolDowns[1] = TickTimer.CreateFromSeconds(Runner, monster.CommonSkillTable[1].CoolDown);

        // �ִϸ��̼��� ������ ������ ���°� �ȹٲ��
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
