using Fusion;

public class Bowmeter_Pattern1 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
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
        //if (_tickTimer.Expired(Runner))
        //{
        //    monster.AIPathing.SetDestination(monster.target.position);
        //    monster.IsAttack = false;
        //    if (!monster.AIPathing.pathPending && !monster.IsDead)
        //    {
        //        if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
        //        {
        //            phase.ChangeState<Bowmeter_Pattern1>();
        //        }
        //        else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
        //        {
        //            phase.ChangeState<Bowmeter_Run>();
        //        }
        //    }
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }
}
