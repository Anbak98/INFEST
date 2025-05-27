using UnityEngine;

public class Stacker_Run : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.IsRun = true;
        monster.CurMovementSpeed = monster.info.SpeedMove;        
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRun = false;
    }
}
