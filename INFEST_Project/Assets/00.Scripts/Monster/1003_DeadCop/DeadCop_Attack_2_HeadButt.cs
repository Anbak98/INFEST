using Fusion;

public class DeadCop_Attack_2_HeadButt : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsAttack = true;
        monster.CurMovementSpeed = 0f;

        //monster.targetStatHandler = monster.target.GetComponent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(10);
        float animTime = monster.GetCurrentAnimLength();
        monster.animTickTimer = TickTimer.CreateFromSeconds(Runner, animTime);
    }

    public override void Execute()
    {
        base.Execute();

        if (monster.animTickTimer.Expired(Runner))
        {
            phase.ChangeState<DeadCop_Idle>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsAttack = false;
    }
}
