using Fusion;

public class Bowmeter_Pattern1 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;        
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
    }
}
