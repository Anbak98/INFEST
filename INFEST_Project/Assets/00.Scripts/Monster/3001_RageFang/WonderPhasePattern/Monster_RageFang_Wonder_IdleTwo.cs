using Fusion;
using UnityEngine;

public class Monster_RageFang_Wonder_IdleTwo : MonsterStateNetworkBehaviour<Monster_RageFang>
{
    [SerializeField]
    private float DurationTime =7.0f;
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, DurationTime);
    }

    public override void Execute()
    {
        base.Execute();

        if(_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Monster_RageFang_Wonder_Walk>();
        }
    }
}
