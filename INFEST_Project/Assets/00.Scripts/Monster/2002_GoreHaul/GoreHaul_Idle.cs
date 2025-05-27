using Fusion;

public class GoreHaul_Idle : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Wonder>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.IsIdle = true;
        monster.CurMovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }

    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            phase.ChangeState<GoreHaul_Walk>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsIdle = false;
    }
}
