using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_DeadCop : MonsterFSM<Monster_DeadCop>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        // ��� phase�� state�� ����ȴ�

        // ���̺� ����(bool������ ������ �� ����)
        //if (���̺� ���۶� bool���� true�� �Ѵٵ簡, �ƴϸ� ���̺�� ���� ������ ���� ���ڰ� ���ڰ� 0�� �ƴ϶��)
        //{
        //    // Wave ���·� ����
        //    monster.FSM.ChangePhase<DeadCop_Phase_Wave>();
        //}

        // ���̺갡 �����ٸ�
        //if (���̺� ���۶� ������ �������� ��� ������)
        //{
        //    // Wander ���·� ���ư���
        //    monster.FSM.ChangePhase<DeadCop_Phase_Wander>();
        //}

        if (monster.IsLookPlayer())
        {
            ChangePhase<DeadCop_Phase_Chase>();
        }
    }
}
