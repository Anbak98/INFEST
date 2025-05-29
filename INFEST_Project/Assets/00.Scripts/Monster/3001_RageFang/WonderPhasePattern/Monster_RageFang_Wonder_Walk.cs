using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Wonder_Walk : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Wonder>
{
    Vector3 randomPosition;

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
        monster.CurMovementSpeed = 0;
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        float minDistance = 5f;
        float maxDistance = 15f;

        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = 0;
            randomDirection *= Random.Range(minDistance, maxDistance);
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 3, NavMesh.AllAreas))
            {
                if(hit.distance < 15f)
                {
                    return hit.position;
                }
            }
        }

        // ��� �õ� ���� �� ���� ��ġ ��ȯ
        return transform.position;
    }
}
