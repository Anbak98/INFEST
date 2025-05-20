public class Bowmeter_Phase_Wonder : MonsterPhase<Monster_Bowmeter>
{
    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsFindPlayer() && !monster.IsDead)
        {
            monster.FSM.ChangePhase<Bowmeter_Phase_Chase>();
        }
    }
}

