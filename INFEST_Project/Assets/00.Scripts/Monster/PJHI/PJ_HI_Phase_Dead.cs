
public class PJ_HI_Phase_Dead : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("PJ_HI_Dying");
    }
}
