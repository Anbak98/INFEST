using Fusion;

public class Stacker_AttackWave : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Wave>
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
           monster.MoveToTarget();
            monster.IsAttack = false;
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.IsTargetInRange(1f))
                {
                    phase.ChangeState<Stacker_AttackWave>();
                }
                else if (monster.IsTargetInRange(1f))
                {
                    phase.ChangeState<Stacker_RunWave>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
