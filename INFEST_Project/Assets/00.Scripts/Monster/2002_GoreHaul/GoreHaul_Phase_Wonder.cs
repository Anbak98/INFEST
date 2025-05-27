public class GoreHaul_Phase_Wonder : MonsterPhase<Monster_GoreHaul>
{
    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsFindPlayer() && !monster.IsDead)
        {
            monster.FSM.ChangePhase<GoreHaul_Phase_Chase>();
        }
    }
}
