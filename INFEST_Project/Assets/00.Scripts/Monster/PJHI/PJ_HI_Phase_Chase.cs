
public class PJ_HI_Phase_Chase : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.AIPathing.speed = monster.info.SpeedMoveWave;

        monster.IsReadyForChangingState = true;
        monster.IsChasePhase = true;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        /// target�� ü���� 0�̸� null�� �����
        if (monster.IsTargetDead())
        {
            monster.target = null;
            // ���ο� ��ǥ�� �����Ѵ�
            monster.SetTargetRandomly();
            // ���� ����Ʈ�� �÷��̾ �ִٸ� Ÿ���� �����ǰ�, ������ �ֺ��� �÷��̾ ������ null�̴�
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<Grita_Phase_Wander>();
            return;
        }

        monster.AIPathing.SetDestination(monster.target.position);

        if(!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= monster.CommonSkillTable[1].UseRange)
                {
                    ChangeState<PJ_HI_Attack>();
                }
                else
                {
                    ChangeState<PJ_HI_Run>();
                }
            }
        }
    }
}
