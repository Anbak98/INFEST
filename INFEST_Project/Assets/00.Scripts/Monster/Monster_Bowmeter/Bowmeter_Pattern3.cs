using Fusion;

public class Bowmeter_Pattern3 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsShoot = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsShoot = false;
    }
}
