
using UnityEngine;

public class Grita_Chase_Attack : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();

        monster.IsReadyForChangingState = false;
        monster.CurMovementSpeed = 0f;
        monster.IsPunch = true;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skill[1].DamageCoefficient));
    }
}
