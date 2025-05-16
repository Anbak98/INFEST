using UnityEngine;

public class PJ_HI_Run : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
