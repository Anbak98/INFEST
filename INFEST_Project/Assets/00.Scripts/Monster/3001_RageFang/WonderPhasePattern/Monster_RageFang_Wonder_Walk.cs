using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Wonder_Walk : MonsterStateNetworkBehaviour<Monster_RageFang>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMove;
        randomPosition = GetRandomPositionOnNavMesh(); // NavMesh ���� ������ ��ġ�� �����ɴϴ�.
        monster.AIPathing.SetDestination(randomPosition); // NavMeshAgent�� ��ǥ ��ġ�� ���� ��ġ�� �����մϴ�.
    }

    public override void Execute()
    {
        base.Execute();
        base.Execute();// ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (!monster.AIPathing.pathPending)
        {
            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<Monster_RageFang_Wonder_Idle>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f; // ���ϴ� ���� ���� ������ ���� ���͸� �����մϴ�.
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
