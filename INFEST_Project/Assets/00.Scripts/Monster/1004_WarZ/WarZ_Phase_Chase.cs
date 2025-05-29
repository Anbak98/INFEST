using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Chase : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();

        monster.animator.Play("Chase.WarZ_Run");
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<WarZ_Chase_Run>(); // currentState�� ������ 1�� ����
    }


    public override void MachineExecute()
    {
        base.MachineExecute();
        if (monster.IsTargetDead())   
        {
            monster.target = null;
            // ���ο� ��ǥ�� �����Ѵ�
            monster.SetTargetRandomly();
            // ���� ����Ʈ�� �÷��̾ �ִٸ� Ÿ���� �����ǰ�, ������ �ֺ��� �÷��̾ ������ null�̴�
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<WarZ_Phase_Wander>();
            return;
        }

       monster.MoveToTarget();

        // �������ڸ��� ���ݵǴ°� ����
        if (!monster.AIPathing.pathPending)
        {
            // �ִϸ��̼��� ������ ���� ���� ��ȯ�� �Ѵ�
            {
                if (monster.IsReadyForChangingState)
                {
                    CaculateAttackType(monster.AIPathing.remainingDistance);

                    switch (nextPatternIndex)
                    {
                        case 0:
                            ChangeState<WarZ_Chase_Run>(); break;
                        case 1:
                            ChangeState<WarZ_Chase_DropKick>(); break;
                        case 2:
                            ChangeState<WarZ_Chase_Punch>(); break;
                        case 3:
                            monster.FSM.ChangePhase<WarZ_Phase_Wander>(); break;
                    }
                }
            }
        }
    }

    public void CaculateAttackType(float distance)
    {

        // ����� �� DropKick, �ָ� Punch �ϸ� �ȴ�
        if (distance <= 1.5)
        {
            // DropKick
            nextPatternIndex = 2;
            return;
        }
        else if (distance < 2.0f)
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
