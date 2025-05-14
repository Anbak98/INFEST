using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// WonderPhase���� ���Ե� ����
public class Grita_Walk : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    Vector3 randomPosition;

    //public GritaPlayerDetector ditector;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
        randomPosition = GetRandomPositionOnNavMesh(); // NavMesh ���� ������ ��ġ�� �����ɴϴ�.
        monster.AIPathing.SetDestination(randomPosition); // NavMeshAgent�� ��ǥ ��ġ�� ���� ��ġ�� �����մϴ�.
    }

    public override void Execute()
    {
        base.Execute();
        if (/*ditector.isTriggered &&*/ monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
            phase.ChangeState<Grita_Scream>();

        if (!monster.AIPathing.pathPending)
        {
            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<Grita_Idle>();
            }
        }
    }
    private Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 3f; // ���ϴ� ���� ���� ������ ���� ���͸� �����մϴ�.
        randomDirection += transform.position; // ���� ���� ���͸� ���� ��ġ�� ���մϴ�.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 3, NavMesh.AllAreas)) // ���� ��ġ�� NavMesh ���� �ִ��� Ȯ���մϴ�.
        {
            return hit.position; // NavMesh ���� ���� ��ġ�� ��ȯ�մϴ�.
        }
        else
        {
            return transform.position; // NavMesh ���� ���� ��ġ�� ã�� ���� ��� ���� ��ġ�� ��ȯ�մϴ�.
        }
    }
}
