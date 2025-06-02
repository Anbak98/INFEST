using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJ_HI_Attack : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        
        monster.CurMovementSpeed = 0f;
        monster.IsReadyForChangingState = false;
        monster.IsPunch = !monster.IsPunch;
    }

    public override void Exit()
    {
        base.Exit();
        monster.SetTargetRandomly();
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget(monster.CurDamage);
    }
}
