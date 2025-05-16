public class Stacker_Phase_Chase : MonsterPhase<Monster_Stacker>
{
    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsReadyForChangingState)
        {
            monster.AIPathing.SetDestination(monster.target.position);
        }
    }
}
