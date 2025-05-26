using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_DropKick : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        monster.IsDropKick = true;

        // �ִϸ��̼��� ������ ������ ���°� �ȹٲ��
        monster.IsReadyForChangingState = false;
    }


    public override void Exit()
    {
        base.Exit();
        monster.IsDropKick = false;
        // �ִϸ��̼��� �����ϴ���
        // Phase_Chase�� currentState�� DropKick���� ���������Ƿ�
        // Run���� �ٲ���Ѵ�
        // phase.ChangeState<WarZ_Chase_Run>(); // overflow �߻�...
        // ChangeState���� currentState?.Exit�� ���ͼ� Exit�� ��� ȣ��Ǳ� �����̴�
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }

}
