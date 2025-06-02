public class GoreHaul_Run : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
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

        if (!monster.AIPathing.pathPending && monster.IsTargetInRange(7f))
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
