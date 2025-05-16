public class Stacker_Phase_Chase : MonsterPhase<Monster_Stacker>
{
    public override void MachineEnter()
    {
        base.MachineEnter();        
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsReadyForChangingState)
        {
            monster.AIPathing.SetDestination(monster.target.position);
        }

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= 2f)
                {
                    ChangeState<Stacker_Attack>();
                }
                else if (monster.AIPathing.remainingDistance > 2f)
                {
                    ChangeState<Stacker_Run>();
                }
            }
        }
    }
}
