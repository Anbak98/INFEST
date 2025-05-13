using Fusion;

public class Bowmeter_Pattern3 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsShoot = true;
        monster.MovementSpeed = 0f;

        //monster.targetStatHandler = monster.target.GetComponent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(10);
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 10);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Bowmeter_Idle>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsShoot = false;
    }
}
