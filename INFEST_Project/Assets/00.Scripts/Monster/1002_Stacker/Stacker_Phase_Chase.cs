public class Stacker_Phase_Chase : MonsterPhase<Monster_Stacker>
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
            monster.TryRemoveTarget(monster.target);
            // ���ο� ��ǥ�� �����Ѵ�
            monster.SetTargetRandomly();
            // ���� ����Ʈ�� �÷��̾ �ִٸ� Ÿ���� �����ǰ�, ������ �ֺ��� �÷��̾ ������ null�̴�
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<Stacker_Phase_Wonder>();
            return;
        }

        if (monster.IsReadyForChangingState)
        {
           monster.MoveToTarget();
        }

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= monster.commonSkill[1].UseRange)
                {
                    ChangeState<Stacker_Attack>();
                }
                else
                {
                    ChangeState<Stacker_Run>();
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsChasePhase = false;
    }
}
