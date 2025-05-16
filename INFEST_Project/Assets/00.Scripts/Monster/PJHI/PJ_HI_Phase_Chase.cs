
public class PJ_HI_Phase_Chase : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.AIPathing.speed = monster.info.SpeedMoveWave;

        monster.IsReadyForChangingState = true;
        monster.IsChasePhase = true;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        if(!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= 2f)
                {
                    ChangeState<PJ_HI_Attack>();
                }
                else
                {
                    ChangeState<PJ_HI_Run>();
                }
            }
        }
    }
}
