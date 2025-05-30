using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Chase : MonsterPhase<Monster_Grita>
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
            monster.target = null;
            // ���ο� ��ǥ�� �����Ѵ�
            monster.SetTargetRandomly();
            // ���� ����Ʈ�� �÷��̾ �ִٸ� Ÿ���� �����ǰ�, ������ �ֺ��� �÷��̾ ������ null�̴�
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<Grita_Phase_Wander>();
            return;
        }

       monster.MoveToTarget();

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= monster.skill[1].UseRange)
                {
                    ChangeState<Grita_Chase_Attack>();
                }
                else
                {
                    ChangeState<Grita_Chase_Run>();
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
