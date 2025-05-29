using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PJ_HI_Walk : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Wonder>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
        randomPosition = GetRandomPositionOnNavMesh(); // NavMesh 위의 랜덤한 위치를 가져옵니다.
        monster.AIPathing.SetDestination(randomPosition); // NavMeshAgent의 목표 위치를 랜덤 위치로 설정합니다.
    }

    public override void Execute()
    {
        base.Execute();// 아직 경로가 계산되지 않았거나 도착한 경우
        if (!monster.AIPathing.pathPending)
        {
            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                    phase.ChangeState<PJ_HI_Idle>();                
            }
        }
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
                if (hit.distance < 15f)
                {
                    return hit.position;
                }
            }
        }

        // 모든 시도 실패 시 현재 위치 반환
        return transform.position;
    }
}
