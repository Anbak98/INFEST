using Fusion;

public class DeadCop_Wave_Attack_1_Wave : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.CurMovementSpeed = 0f;
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
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
                {
                    phase.ChangeState<DeadCop_Wave_Attack_1_Wave>();
                }
                else
                {
                    monster.IsAttack = false;
                    phase.ChangeState<DeadCop_Wave_RunWave>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
