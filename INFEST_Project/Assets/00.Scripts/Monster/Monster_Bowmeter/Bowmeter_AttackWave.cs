using Fusion;

public class Bowmeter_AttackWave : MonsterStateNetworkBehaviour<Monster_Bowmeter>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.MovementSpeed = 0f;
        monster.IsAttack = true;

        //monster.targetStatHandler = monster.target.GetComponentInParent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(Random.Range(monster.info.MinAtk, monster.info.MaxAtk));
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
                    phase.ChangeState<Bowmeter_AttackWave>();
                }
                else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<Bowmeter_RunWave>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
