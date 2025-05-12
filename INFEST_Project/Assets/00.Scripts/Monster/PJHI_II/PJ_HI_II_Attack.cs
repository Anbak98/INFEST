using Fusion;

public class PJ_HI_II_Attack : MonsterStateNetworkBehaviour<Monster_PJ_HI_II>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead)
            return;
        monster.MovementSpeed = 0f;
        monster.IsAttack = true;
        //monster.targetStatHandler = monster.target.GetComponent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(10);
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void Execute()
    {
        base.Execute();
        if (_tickTimer.Expired(Runner))
        {
            monster.AIPathing.SetDestination(monster.target.position);
            monster.IsAttack = false;
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<PJ_HI_II_Phase_Chase>();
                }
                else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<PJ_HI_II_Run>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
