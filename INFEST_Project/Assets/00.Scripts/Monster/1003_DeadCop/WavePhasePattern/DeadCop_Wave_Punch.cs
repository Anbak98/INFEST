using Fusion;
using UnityEngine;

public class DeadCop_Wave_Punch : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        monster.IsRightPunch = true;

        Debug.Log("Punch");

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
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }
}
