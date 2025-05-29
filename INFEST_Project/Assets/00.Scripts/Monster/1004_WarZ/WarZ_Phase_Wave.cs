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
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<WarZ_Wave_Run>(); // currentState�� ������ 1�� ����

        // �÷��̾��� �νĹ��� �ø���
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
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
        // �ʹ� �ְų� Wander�� ���ư����Ѵ�       
        if (distance > 10f)
        {
            /// Wander -> Idle
            monster.TryRemoveTarget(monster.target);    // Wander���� �̵��Ҷ��� target�� �ƴ϶� randomPosition���� �̵��ϴϱ� null���� �߻����� �ʴ´�
            nextPatternIndex = 3;
            return;
        }

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
        /// Wave �ð� ���� �ʰ��ϸ� ���� ���·� ���ư���
    }
}
