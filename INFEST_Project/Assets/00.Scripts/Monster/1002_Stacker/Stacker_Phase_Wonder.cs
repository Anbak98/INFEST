public class Stacker_Phase_Wonder : MonsterPhase<Monster_Stacker>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsWonderPhase = true;
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsWonderPhase = false;
    }
}
