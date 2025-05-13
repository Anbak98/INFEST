using Fusion;

public class Bowmeter_Idle : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Wonder>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Bowmeter_Walk>();
        }
    }
}
