using Fusion;

public class Bowmeter_Dead : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Dead>
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
