using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// WonderPhase에만 포함된 상태
public class Grita_Walk : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    Vector3 randomPosition;

    //public GritaPlayerDetector ditector;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
        randomPosition = GetRandomPositionOnNavMesh(); // NavMesh 위의 랜덤한 위치를 가져옵니다.
        monster.AIPathing.SetDestination(randomPosition); // NavMeshAgent의 목표 위치를 랜덤 위치로 설정합니다.
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
        Vector3 randomDirection = Random.insideUnitSphere * 3f; // 원하는 범위 내의 랜덤한 방향 벡터를 생성합니다.
        randomDirection += transform.position; // 랜덤 방향 벡터를 현재 위치에 더합니다.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 3, NavMesh.AllAreas)) // 랜덤 위치가 NavMesh 위에 있는지 확인합니다.
        {
            return hit.position; // NavMesh 위의 랜덤 위치를 반환합니다.
        }
        else
        {
            return transform.position; // NavMesh 위의 랜덤 위치를 찾지 못한 경우 현재 위치를 반환합니다.
        }
    }
}
