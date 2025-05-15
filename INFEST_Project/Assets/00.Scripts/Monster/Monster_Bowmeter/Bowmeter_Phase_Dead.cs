public class Bowmeter_Phase_Dead : MonsterPhase<Monster_Bowmeter>
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
