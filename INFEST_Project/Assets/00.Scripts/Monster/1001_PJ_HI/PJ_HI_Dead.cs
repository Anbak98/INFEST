using Fusion;
using UnityEngine;

public class PJ_HI_Dead : MonsterStateNetworkBehaviour
{
    public TickTimer _tickTimer;
    public NetworkObject obj;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }

    public override void Execute()
    {
        base.Execute();

        if(_tickTimer.Expired(Runner))
        {
            if(HasStateAuthority)
            {
                Runner.Despawn(obj);
            }
        }
    }
}
