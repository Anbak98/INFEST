using UnityEngine;

public class Stacker_Run : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.IsRun = true;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;        
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRun = false;
    }
}
