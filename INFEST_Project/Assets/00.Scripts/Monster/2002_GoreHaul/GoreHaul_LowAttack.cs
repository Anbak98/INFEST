using Fusion;
using UnityEngine;

public class GoreHaul_LowAttack : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsLowAttack = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, monster.skills[2].CoolDown);
    }

    public override void Execute()
    {
        base.Execute();

        //if (monster.target == null)
        //    return;

        //float distance = Vector3.Distance(monster.transform.position, monster.target.position);
                
        //if (distance > monster.skills[2].UseRange)
        //{
        //    phase.ChangeState<GoreHaul_Run>();
        //}
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsLowAttack = false;
    }        

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[2].DamageCoefficient));
    }
}
