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
        if (monster.target == null)
            monster.FSM.ChangePhase<WarZ_Phase_Wander>(); 

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
        // Run������ ��� ���·� ��ȯ�� �����ϴ�
        // Ÿ���� �÷��̾ ���ų�(?) ���ʿ� Ÿ���� �÷��̾ �־ Chase�� ���°Ŵϱ�

        // �ʹ� �ְų�, Ÿ���� ü���� 0�̰ų�(bool������ 0ó��) Ÿ���� null�� �ϰ� Wander�� ���ư����Ѵ�       
        if (distance > 10f /*|| Ÿ���� ü�� 0 */)
        {
            /// Wander -> Idle
            monster.TryRemoveTarget(monster.target);    // Wander���� �̵��Ҷ��� target�� �ƴ϶� randomPosition���� �̵��ϴϱ� null���� �߻����� �ʴ´�
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
            //ChangeState<WarZ_Chase_Run>();
            nextPatternIndex = 0;
        }

        // Ÿ���� �ٲ��ִ� ����
        // Ÿ���� ü��0 üũ
        // Ÿ�ٿ� �ٸ� ���Ͱ� �ִٸ�
        // ������ �˾Ƽ� idle�� �ǰ���
        //monster.SetTargetRandomly();

    }
}
