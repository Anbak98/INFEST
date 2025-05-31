
using UnityEngine;
using UnityEngine.AI;

public class PJ_HI_Phase_Chase : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;

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
            monster.FSM.ChangePhase<PJ_HI_Phase_Wonder>();
            return;
        }

       monster.MoveToTarget();

        if(!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.IsTargetInRange(monster.CommonSkillTable[1].UseRange))
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

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsChasePhase = false;
    }
}
