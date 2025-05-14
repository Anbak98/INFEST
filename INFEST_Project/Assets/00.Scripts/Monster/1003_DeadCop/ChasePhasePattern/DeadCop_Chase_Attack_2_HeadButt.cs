using Fusion;

public class DeadCop_Chase_Attack_2_HeadButt : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Chase>
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
            monster.AIPathing.SetDestination(monster.target.position);
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance <= 0.5f)
                {
                    phase.ChangeState<DeadCop_Chase_Attack_2_HeadButt>();
                }
                else
                {
                    monster.IsAttack = false;
                    phase.ChangeState<DeadCop_Chase_Run>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
