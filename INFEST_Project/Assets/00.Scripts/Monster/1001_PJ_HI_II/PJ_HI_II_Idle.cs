using Fusion;

public class PJ_HI_II_Idle : MonsterStateNetworkBehaviour
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
            phase.ChangeState<PJ_HI_II_Walk>();
        }
    }
}
