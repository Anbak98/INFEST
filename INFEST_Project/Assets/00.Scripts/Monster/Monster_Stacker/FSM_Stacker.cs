public class FSM_Stacker : MonsterFSM<Monster_Stacker>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (monster.IsLookPlayer())
        {
            ChangePhase<Stacker_Phase_Chase>();
        }
    }
}
