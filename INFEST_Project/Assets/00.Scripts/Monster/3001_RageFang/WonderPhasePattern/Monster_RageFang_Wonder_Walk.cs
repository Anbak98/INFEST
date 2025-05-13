using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Wonder_Walk : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Wonder>
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
        float minDistance = 10f;
        float maxDistance = 20f;

        // ���� ���͸� ���ϰ�, �ּ�~�ִ� �Ÿ� ������ ���� �Ÿ���ŭ ���մϴ�.
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0; // ����鿡���� �̵��ϵ��� y���� 0���� ����ϴ�.
        randomDirection *= Random.Range(minDistance, maxDistance);
        randomDirection += transform.position;

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
