public class FSM_PJ_HI_II : MonsterFSM<Monster_PJ_HI_II>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (monster.IsLookPlayer())
        {
            ChangePhase<PJ_HI_II_Phase_Chase>();
        }
    }
}
