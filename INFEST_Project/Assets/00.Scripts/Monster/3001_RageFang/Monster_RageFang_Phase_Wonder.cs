using Mono.Cecil;

public class Monster_RageFang_Phase_Wonder : MonsterPhase<Monster_RageFang>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.PhaseIndex = 0;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        if (monster.target != null)
        {
            monster.FSM.ChangePhase<Monster_RageFang_Phase_AttackOne>();
        }
    }
}
