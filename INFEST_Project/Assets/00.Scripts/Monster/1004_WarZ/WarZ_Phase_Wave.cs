using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Wave : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Wave.WarZ_Run");
        // �÷��̾��� �νĹ��� �ø���
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        //if (monster.PlayerDetectorCollider.radius != monster.info.DetectAreaWave)
        //    monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;

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
                        ChangeState<WarZ_Wave_Run>(); break;
                    case 1:
                        ChangeState<WarZ_Wave_DropKick>(); break;
                    case 2:
                        ChangeState<WarZ_Wave_Punch>(); break;
                    case 3:
                        monster.FSM.ChangePhase<WarZ_Phase_Wander>(); break;
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsWave = false;
        // �÷��̾��� �νĹ��� ���δ�
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaNormal;
    }

    public void CaculateAttackType(float distance)
    {
        // �޸����� ��� ���·� ��ȯ�� �����ϴ�
        // ������ Wander�� ���ư���, 
        // ���� ��쿡�� ����� �� DropKick, �ָ� Punch �ϸ� �ȴ�
        if (distance <= 0.5)
        {
            // DropKick
            nextPatternIndex = 2;
        }
        else if (distance > 0.5f && distance < 1.0f)
        {
            // Punch
            nextPatternIndex = 1;
        }
        else if (distance > 1.0f && distance < 10f)
        {
            // Run
            nextPatternIndex = 0;
        }
        /// �Ƹ� Wave �ð� ���� �ʰ������� ������...?
        else if (distance > 10f)
        {

            // Wander -> Idle
            nextPatternIndex = 3;
        }
    }


}
