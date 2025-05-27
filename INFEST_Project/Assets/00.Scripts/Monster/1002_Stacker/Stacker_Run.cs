using UnityEngine;

public class Stacker_Run : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.IsRun = true;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
        monster.SetTargetRandomly();
    }

    public override void Execute()
    {
        base.Execute();
        //monster.AIPathing.SetDestination(monster.target.position);

        if (!monster.AIPathing.pathPending && monster.AIPathing.remainingDistance <= 2f)
        {
            monster.IsReadyForChangingState = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRun = false;
    }
}
