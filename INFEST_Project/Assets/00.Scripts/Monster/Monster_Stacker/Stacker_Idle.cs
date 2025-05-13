using Fusion;

public class Stacker_Idle : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Wonder>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Stacker_Walk>();
        }
    }
}
