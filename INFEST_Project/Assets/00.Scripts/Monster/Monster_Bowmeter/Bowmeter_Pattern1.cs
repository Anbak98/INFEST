using Fusion;

public class Bowmeter_Pattern1 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsAttack = true;
        monster.CurMovementSpeed = 0f;        

        //monster.targetStatHandler = monster.target.GetComponent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(10);
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsAttack = false;
    }
}
