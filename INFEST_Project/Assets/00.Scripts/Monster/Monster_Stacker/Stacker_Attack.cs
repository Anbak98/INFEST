using Fusion;

public class Stacker_Attack : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null || !monster.AIPathing.pathPending)
            return;

        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void Execute()
    {
        base.Execute();
        //if (_tickTimer.Expired(Runner) && monster.IsReadyForChangingState)
        //{
        //    monster.AIPathing.SetDestination(monster.target.position);
        //    if (!monster.AIPathing.pathPending && !monster.IsDead)
        //    {
        //        if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
        //        {
        //            phase.ChangeState<Stacker_Attack>();
        //        }
        //        else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
        //        {
        //            phase.ChangeState<Stacker_Run>();
        //        }
        //    }
        //}
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
        //monster.IsReadyForChangingState = false;
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget(monster.info.MinAtk);
    }
}
