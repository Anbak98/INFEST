using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Chase : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int nextPatternIndex = 0;
    public TickTimer[] CoolDown = new TickTimer[3];

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
            monster.TryRemoveTarget(monster.target);
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
                    CaculateAttackType();

                    switch (nextPatternIndex)
                    {
                        case 0:
                            ChangeState<WarZ_Chase_Run>(); break;
                        case 1:
                            ChangeState<WarZ_Chase_Punch>(); break;
                        case 2:
                            ChangeState<WarZ_Chase_DropKick>(); break;
                        case 3:
                            monster.FSM.ChangePhase<WarZ_Phase_Wander>(); break;
                    }
                }
            }
        }
    }

    public void CaculateAttackType()
    {
        // ����� �� DropKick, �ָ� Punch �ϸ� �ȴ�
        if (CoolDown[2].ExpiredOrNotRunning(Runner))
        {
            if (monster.IsTargetInRange(monster.CommonSkillTable[3].UseRange))
            {
                // DropKick
                nextPatternIndex = 2;
            }
            else
            {
                nextPatternIndex = 0;
            }
        }
        else if (CoolDown[1].ExpiredOrNotRunning(Runner))
        {
            if (monster.IsTargetInRange(monster.CommonSkillTable[1].UseRange))
            {
                nextPatternIndex = 1;
            }
            else
            {
                nextPatternIndex = 0;
            }
        }
        else
        {
            // Run
            nextPatternIndex = 0;
        }
    }
}
