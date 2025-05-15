using Fusion;

public class Stacker_Attack : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();

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
        monster.TryAttackTarget(monster.CurDamage);
    }
}
