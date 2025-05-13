using Fusion;
using UnityEngine;

public class PJ_HI_Idle : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Wonder>
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

        if(_tickTimer.Expired(Runner))
        {
            phase.ChangeState<PJ_HI_Walk>();
        }
    }
}
