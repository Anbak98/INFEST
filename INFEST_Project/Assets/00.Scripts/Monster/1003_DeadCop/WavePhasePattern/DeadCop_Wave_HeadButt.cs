using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Wave_HeadButt : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();

        monster.CurMovementSpeed = 0f;
        monster.IsHeadButt = true;

        // �ִϸ��̼��� ������ ������ ���°� �ȹٲ��
        monster.IsReadyForChangingState = false;
    }


    public override void Exit()
    {
        base.Exit();
        monster.IsHeadButt = false;
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }


}
