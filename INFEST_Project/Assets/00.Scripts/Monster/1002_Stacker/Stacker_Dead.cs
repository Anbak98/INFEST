using Fusion;

public class Stacker_Dead : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Dead>
{
    public TickTimer _tickTimer;
    public NetworkObject obj;

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
            if (HasStateAuthority)
            {
                Runner.Despawn(obj);
            }
        }
    }
}
