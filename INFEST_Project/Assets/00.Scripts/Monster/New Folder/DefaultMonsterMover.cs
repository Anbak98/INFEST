using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Infest.Monster
{
    public class DefaultMonsterMover : IMonsterMoveHandler
    {
        private Transform target;

        private readonly Transform transform;
        private readonly NavMeshAgent agent;
        private readonly NetworkRunner runner;

        private readonly bool HasStateAuthority = false;

        public DefaultMonsterMover(Transform transform, NavMeshAgent agent, NetworkRunner runner)
        {
            this.transform = transform;
            this.agent = agent;
            this.runner = runner;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void Move()
        {
            if (target == null || !HasStateAuthority)
                return;

            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > 1f)
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(target.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    debugPath = path;

                    if (path.corners.Length > 1)
                    {
                        Vector3 nextCorner = path.corners[1];
                        Vector3 moveDirection = (nextCorner - transform.position);
                        moveDirection.y = 0;

                        Vector3 normalizedDirection = moveDirection.normalized;
                        agent.Move(normalizedDirection * agent.speed * runner.DeltaTime);
                        transform.rotation = Quaternion.LookRotation(normalizedDirection);

                        // 디버그용 선 그리기
                        //Debug.DrawLine(transform.position, nextCorner, Color.green);
                    }
                    else
                    {
                        Vector3 moveDirection = (target.position - transform.position);
                        moveDirection.y = 0;

                        Vector3 normalizedDirection = moveDirection.normalized;
                        agent.Move(normalizedDirection * agent.speed * runner.DeltaTime);
                    }
                }
                else
                {
                    debugPath = path;
                    //Debug.LogWarning($"Path status: {path.status}");

                    //Mounting mount = FindAnyObjectByType<Mounting>();
                    //if (mount != null)
                    //{
                    //    TryAddTarget(mount.transform);
                    //    TrySetTarget(mount.transform);
                    //}
                }
            }
        }


        private Vector3 lastTargetPosition;
        private Vector3? randomDestination = null;

        public bool MoveRandomly(float minDistance, float maxDistance, float radius)
        {
            if (randomDestination == null || Vector3.Distance(transform.position, randomDestination.Value) < 0.5f)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 randomDirection = Random.onUnitSphere;
                    randomDirection.y = 0;
                    randomDirection *= Random.Range(minDistance, maxDistance);
                    Vector3 samplePosition = transform.position + randomDirection;

                    if (NavMesh.SamplePosition(samplePosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
                    {
                        Vector3 candidate = hit.position;

                        // 실제 경로 길이 계산
                        float pathLength;

                        NavMeshPath path = new NavMeshPath();
                        if (NavMesh.CalculatePath(transform.position, candidate, NavMesh.AllAreas, path))
                        {
                            float length = 0.0f;
                            for (int j = 1; j < path.corners.Length; j++)
                            {
                                length += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                            }
                            pathLength = length;
                        }
                        else
                        {
                            pathLength = float.MaxValue; // 경로 계산 실패 시 매우 큰 값 반환
                        }

                        if (pathLength >= minDistance && pathLength <= maxDistance)
                        {
                            randomDestination = candidate;
                            break; // 유효한 경로 거리면 선택
                        }
                    }
                }

                if (randomDestination == null)
                {
                    randomDestination = transform.position;
                }
            }

            if (randomDestination.HasValue)
            {
                Vector3 direction = (randomDestination.Value - transform.position);
                direction.y = 0;

                if (direction.magnitude > 0.1f)
                {
                    agent.Move(direction.normalized * agent.speed * runner.DeltaTime);
                    transform.rotation = Quaternion.LookRotation(direction); // 회전 적용
                }
            }

            if (Vector3.Distance(transform.position, randomDestination.Value) < 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

#if UNITY_EDITOR
        private NavMeshPath debugPath;

        private void OnDrawGizmos()
        {
            if (debugPath != null && debugPath.corners.Length > 1)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < debugPath.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(debugPath.corners[i], debugPath.corners[i + 1]);
                }
            }
        }
    }
#endif
}