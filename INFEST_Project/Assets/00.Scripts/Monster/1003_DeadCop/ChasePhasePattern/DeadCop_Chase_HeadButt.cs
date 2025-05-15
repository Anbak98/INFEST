using Fusion;

public class DeadCop_Chase_HeadButt : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();

        monster.CurMovementSpeed = 0f;
        monster.IsHeadButt= true;

        // �ִϸ��̼��� ������ ������ ���°� �ȹٲ��
        monster.IsReadyForChangingState = false;
    }


    public override void Exit()
    {
        base.Exit();
        monster.IsHeadButt= false;
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }

}
