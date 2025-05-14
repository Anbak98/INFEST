using Fusion;

public class Bowmeter_Pattern2 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsBwack = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, 7f);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsBwack = false;
    }
}
