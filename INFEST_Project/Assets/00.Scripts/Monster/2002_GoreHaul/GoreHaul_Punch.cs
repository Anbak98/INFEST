using Fusion;
using UnityEngine;

public class GoreHaul_Punch : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[1] = TickTimer.CreateFromSeconds(Runner, monster.skills[1].CoolDown);
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[1].DamageCoefficient));
    }
}
