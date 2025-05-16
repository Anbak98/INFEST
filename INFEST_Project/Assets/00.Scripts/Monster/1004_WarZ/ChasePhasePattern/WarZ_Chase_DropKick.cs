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
        phase.ChangeState<WarZ_Chase_Run>();
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }

}
