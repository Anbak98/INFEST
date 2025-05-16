public class Stacker_Phase_Chase : MonsterPhase<Monster_Stacker>
{
    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsReadyForChangingState)
        {
            monster.SetTargetRandomly();
            monster.SetTarget(monster.target.transform);
            monster.AIPathing.SetDestination(monster.target.position);

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                ChangeState<Stacker_Attack>();
            }
            else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
            {
                ChangeState<Stacker_Run>();
            }
        }
    }
}
