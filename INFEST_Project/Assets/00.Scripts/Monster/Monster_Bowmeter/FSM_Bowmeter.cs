public class FSM_Bowmeter : MonsterFSM<Monster_Bowmeter>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (monster.IsLookPlayer())
        {
            ChangePhase<Bowmeter_Phase_Chase>();
        }
    }
}
