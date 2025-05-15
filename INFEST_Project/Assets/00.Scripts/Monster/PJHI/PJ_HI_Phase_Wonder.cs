public class PJ_HI_Phase_Wonder : MonsterPhase<Monster_PJ_HI>
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
