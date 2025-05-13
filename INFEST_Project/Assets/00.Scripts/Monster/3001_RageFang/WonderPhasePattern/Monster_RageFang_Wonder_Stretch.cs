using Fusion;

public class Monster_RageFang_Wonder_Stretch : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Wonder>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.IsStretch = true;
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 3);
    }

    public override void Execute()
    {
        base.Execute();

        if(_tickTimer.Expired(Runner))
        {
            phase.ChangeState<Monster_RageFang_Wonder_IdleTwo>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsStretch = false;
    }
}
