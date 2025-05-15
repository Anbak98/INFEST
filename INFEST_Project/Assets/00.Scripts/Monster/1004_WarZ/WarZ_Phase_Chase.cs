using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Chase : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        //monster.IsReadyForChangingState = false;
        monster.PhaseIndex = 1;

        Debug.Log("Chase");
        monster.animator.Play("Chase.WarZ_Run");
    }


    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

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

    public void CaculateAttackType(float distance)
    {
        // �޸����� ��� ���·� ��ȯ�� �����ϴ�
        // �ʹ� �ְų� ������ Wander�� ���ư���
        if (distance > 10f /*|| monster.CurMovementSpeed <= 1*/)
        {
            /// Wander -> Idle
            nextPatternIndex = 3;
            return;
        }

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
            ChangeState<WarZ_Chase_Run>();
        }
    }
}
