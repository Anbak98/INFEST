public class Stacker_Phase_Dead : MonsterPhase<Monster_Stacker>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Dead");
        monster.CurMovementSpeed = 0;
    }
}
