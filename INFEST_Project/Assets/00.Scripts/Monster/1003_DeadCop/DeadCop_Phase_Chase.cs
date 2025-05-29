using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Chase : MonsterPhase<Monster_DeadCop>
{
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Chase.DeadCop_Run");
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<DeadCop_Chase_Run>(); // currentState�� ������ 1�� ����
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
            monster.FSM.ChangePhase<DeadCop_Phase_Wander>();
            return;
        }

       monster.MoveToTarget();

        // �������ڸ��� ���ݵǴ°� ����
        if (!monster.AIPathing.pathPending)
        {
            // �ִϸ��̼��� ������ ���� ���� ��ȯ�� �Ѵ�
            {
                if (monster.IsReadyForChangingState)
                    CaculateAttackType(monster.AIPathing.remainingDistance);

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<DeadCop_Chase_Run>(); break;
                    case 1:
                        ChangeState<DeadCop_Chase_HeadButt>(); break;
                    case 2:
                        ChangeState<DeadCop_Chase_Punch>(); break;
                    case 3:
                        monster.FSM.ChangePhase<DeadCop_Phase_Wander>(); break;
                }
            }
        }
    }
    public void CaculateAttackType(float distance)
    {
        // �ʹ� �ְų� Wander�� ���ư����Ѵ�       
        //if (distance > 10f)
        //{
        //    /// Wander -> Idle
        //    monster.TryRemoveTarget(monster.target);    // Wander���� �̵��Ҷ��� target�� �ƴ϶� randomPosition���� �̵��ϴϱ� null���� �߻����� �ʴ´�
        //    nextPatternIndex = 3;
        //    return;
        //}

        // ����� �� DropKick, �ָ� Punch �ϸ� �ȴ�
        if (distance <= 0.5)
        {
            // DropKick
            nextPatternIndex = 2;
            return;
        }
        else if (distance > 0.5f && distance < 1.0f)
        {
            // Punch
            nextPatternIndex = 1;
            return;
        }
        else
        {
            // Run
            nextPatternIndex = 0;
        }
    }

}
