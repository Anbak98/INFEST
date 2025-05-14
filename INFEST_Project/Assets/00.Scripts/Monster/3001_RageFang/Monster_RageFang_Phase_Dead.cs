
public class Monster_RageFang_Phase_Dead : MonsterPhase<Monster_RageFang>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Dead");
        monster.CurMovementSpeed = 0;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
    }
}
