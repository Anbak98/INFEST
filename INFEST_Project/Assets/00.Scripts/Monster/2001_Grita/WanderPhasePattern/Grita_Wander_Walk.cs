using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// WonderPhase���� ���Ե� ����
public class Grita_Wander_Walk : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();
        base.Execute();// ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5f, 10f, 10f))
            {
                phase.ChangeState<Grita_Wander_Idle>();
            }
        }
    }
}
