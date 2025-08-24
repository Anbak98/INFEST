using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNetworkBehaviour : NetworkBehaviour
{
    //   key": 1001,
    //  "Name": "PJ_H I",
    //  "MonsterType": 0,
    //  "MinHealth": 150,
    //  "MaxHealth": 250,
    //  "HealthPer5Min": 25,
    //  "MinAtk": 30,
    //  "MaxAtk": 42,
    //  "AtkPer5Min": 3,
    //  "MinDef": 20,
    //  "MaxDef": 28,
    //  "DefPer5Min": 2,
    //  "SpeedMove": 1.7,
    //  "SpeedMoveWave": 2.5,
    //  "SpeedAtk": 0.7,
    //  "DetectAreaNormal": 5,
    //  "DetectAreaWave": 100,
    //  "State": 200,
    //  "DropGold": 30,
    //  "FieldSpawn": true,
    //  "LimitSpawnCount": 9999
    public MonsterInfo info { get; private set; }

    [Header("Monster Number depends on Data Table")]
    public int key = -1;

    [Header("Monster Control Helper")]
    [Tooltip("Reference to the NavMeshAgent used to determine where the enemy should move to.")]
    public NavMeshAgent AIPathing;
    public SphereCollider PlayerDetectorCollider;

    [Header("Monster Target Helper")]
    [ReadOnly] public Transform target;
    [ReadOnly, SerializeField] private List<Transform> targets = new();
    private Dictionary<Transform, TargetableFromMonster> targetBridges = new();

    [Header("Monster Render Helper")]
    public Animator animator;
    public GameObject hitEffectPrefab;

    [Header("Monster Ragdoll Helper")]
    [SerializeField] private Rigidbody[] ragdollRigidbodys;
    [SerializeField] private BoxCollider[] ragdollBoxCollider;
    [SerializeField] private SphereCollider[] ragdollSphereCollider;
    [SerializeField] private CapsuleCollider[] ragdollCapsuleCollider;

    [field: Header("Monster Status")]
    [Networked, OnChangedRender(nameof(OnChangedMovementSpeed))] public float CurMovementSpeed { get; set; }
    [Networked, OnChangedRender(nameof(OnChangedDetectorRadiusSpeed))] public float CurDetectorRadius { get; set; }
    [field: SerializeField] public int CurHealth { get; set; } = -1;
    [field: SerializeField] public int CurDamage { get; set; }
    [field: SerializeField] public int CurDef { get; set; }
    [field: SerializeField] public int BaseHealth { get; set; } = -1;
    [field: SerializeField] public int BaseDamage { get; set; }
    [field: SerializeField] public int BaseDef { get; set; }
    [field: SerializeField] public int OffsetHealth { get; set; } = -1;
    [field: SerializeField] public int OffsetDamage { get; set; }
    [field: SerializeField] public int OffsetDef { get; set; }
    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnDead))] public NetworkBool IsDead { get; set; } = false;

    public override void Spawned()
    {
        info = DataManager.Instance.GetByKey<MonsterInfo>(key);
        AudioManager.instance.PlaySfx(Sfxs.ZombieSpawn);

        int playerCount = Runner.SessionInfo.PlayerCount - 1 > 0 ? Runner.SessionInfo.PlayerCount - 1 : 0;
        int elapsedTime = (int)(Runner.SimulationTime / 300);

        BaseHealth = (int)(info.MinHealth * (1 + info.HPCoef * playerCount));
        BaseDamage = (int)(info.MinAtk * (1 + info.AtkCoef * playerCount));
        BaseDef = (int)(info.MinDef * (1 + info.DefCoef * playerCount));

        OffsetHealth = (int)(info.HealthPer5Min * elapsedTime);
        OffsetDamage = (int)(info.AtkPer5Min * elapsedTime);
        OffsetDef = (int)(info.DefPer5Min * elapsedTime);

        CurHealth = BaseHealth + OffsetHealth;
        CurDamage = BaseDamage + OffsetDamage;
        CurDef = BaseDef + OffsetDef;

        CurMovementSpeed = info.SpeedMove;
        CurDetectorRadius = info.DetectAreaNormal;

        AIPathing.enabled = true;
        AIPathing.speed = info.SpeedMove;
        ActivateRagdoll(false);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
    }

    private Vector3 lastTargetPosition;
    private Vector3? randomDestination = null;

    public bool MoveToRandomPositionAndCheck(float minDistance, float maxDistance, float radius)
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
                AIPathing.Move(direction.normalized * AIPathing.speed * Runner.DeltaTime);
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

    private NavMeshPath debugPath;
    public void MoveToTarget()
    {
        if (target == null || !HasStateAuthority)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 1f)
        {
            NavMeshPath path = new NavMeshPath();

            if (AIPathing.CalculatePath(target.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                debugPath = path;

                if (path.corners.Length > 1)
                {
                    Vector3 nextCorner = path.corners[1];
                    Vector3 moveDirection = (nextCorner - transform.position);
                    moveDirection.y = 0;

                        Vector3 normalizedDirection = moveDirection.normalized;
                        AIPathing.Move(normalizedDirection * AIPathing.speed * Runner.DeltaTime);
                        transform.rotation = Quaternion.LookRotation(normalizedDirection);

                        // 디버그용 선 그리기
                        Debug.DrawLine(transform.position, nextCorner, Color.green);                    
                }
                else
                {
                    Vector3 moveDirection = (target.position - transform.position);
                    moveDirection.y = 0;

                    Vector3 normalizedDirection = moveDirection.normalized;
                    AIPathing.Move(normalizedDirection * AIPathing.speed * Runner.DeltaTime);
                }
            }
            else
            {
                debugPath = path;
                Debug.LogWarning($"Path status: {path.status}");

                Mounting mount = FindAnyObjectByType<Mounting>();
                if (mount != null)
                {
                    TryAddTarget(mount.transform);
                    TrySetTarget(mount.transform);
                }
            }
        }
    }

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


        //if (target != null)
        //{
        //    NavMeshPath path = new NavMeshPath();
        //    Debug.Log(AIPathing.velocity.magnitude);
        //    AIPathing.velocity *= Runner.DeltaTime * 100;
        //    if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path)
        //        && path.status == NavMeshPathStatus.PathComplete)
        //    {
        //        if (Vector3.Distance(lastTargetPosition, target.position) > 10f)
        //        {
        //            Debug.Log("Re");
        //            AIPathing.SetDestination(target.position);
        //            lastTargetPosition = target.position;
        //        }
        //    }
        //    else
        //    {
        //        Mounting mount = FindAnyObjectByType<Mounting>();
        //        if (mount != null)
        //        {
        //            TryAddTarget(mount.transform);
        //            TrySetTarget(mount.transform);
        //        }
        //    }
        //}
    

    public bool IsLookPlayer()
    {
        foreach (var _target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(dirToTarget, transform.forward.normalized);
            if (dot > Mathf.Cos(30f * Mathf.Deg2Rad))
            {
                target = _target;
                return true;
            }
        }

        return false;
    }

    public bool IsFindPlayer()
    {
        if (targets.Count > 0)
        {
            SetTargetRandomly();
            return true;
        }

        return false;
    }

    public bool IsTargetDead()
    {
        if (target.IsDestroyed())
            return true;

        if (target != null)
        {
            if (targetBridges.TryGetValue(target, out var bridge))
            {
                if (bridge.CurHealth <= 0)
                    return true;
            }
        }

        return false;
    }

    public bool IsTargetInRange(float range)
    {
        return Vector3.Distance(transform.position, target.position) < range;
    }

    public void SetTargetRandomly()
    {
        if (targets.Count > 0)
        {
            Transform newTarget;
            newTarget = targets[Random.Range(0, targets.Count)];
            if (newTarget != null)
                target = newTarget;
            else
            {
                TryRemoveTarget(newTarget);
                SetTargetRandomly();
            }
        }
    }

    public void TrySetTarget(Transform newTarget)
    {
        if (targets.Contains(newTarget))
            this.target = newTarget;
    }

    public void TryAddTarget(Transform newTarget)
    {
        Debug.Log(newTarget);
        if (!targets.Contains(newTarget))
        {
            if (newTarget.TryGetComponent(out TargetableFromMonster bridge))
            {
                Debug.Log(newTarget);
                targets.Add(newTarget);
                targetBridges.Add(newTarget, bridge);
            }
        }
    }

    public void TryRemoveTarget(Transform target)
    {
        if (this.target == target)
        {
            Debug.Log("Removed");
            this.target = null;

            if (targets.Contains(target))
            {
                targets.Remove(target);
                targetBridges.Remove(target);
            }
        }
    }

    public void TryAttackTarget(int damage)
    {
        if (targetBridges.TryGetValue(target, out TargetableFromMonster bridge))
        {
            bridge.ApplyDamage(this, damage);
        }
    }

    public void TryAttackTarget(TargetableFromMonster bridge, int damage)
    {
        bridge.ApplyDamage(this, damage);
    }

    public IState GetTryTargetState(Transform target)
    {
        if(targetBridges.TryGetValue(target, out TargetableFromMonster tfm))
        {
            return tfm.CurState;
        }

        return null;
    }

    private void OnChangedMovementSpeed()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        AIPathing.speed = CurMovementSpeed;
    }

    private void OnChangedDetectorRadiusSpeed()
    {
        PlayerDetectorCollider.radius = CurDetectorRadius;
    }

    public virtual bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
        if (isCritical)
            Debug.Log("HeadShot");
        if (CurHealth <= 0f)
            return false;

        int dmg = (int)damage - CurDef;

        if (dmg <= 0)
            dmg = 1;

        CurHealth -= dmg;

        if (CurHealth <= 0f)
        {
            CurHealth = 0;
            NetworkGameManager.Instance.gamePlayers.AddKillCount(instigator, 1);
            NetworkGameManager.Instance.gamePlayers.AddGoldCount(instigator, info.DropGold);


            IsDead = true;

            if (weaponType == EWeaponType.Launcher)
            {
                RPC_RagdollEffect(position);
            }


        }

        Vector3 dir = (transform.position - position).normalized;

        // Store relative hit position.
        // Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.

        RPC_PlayDamageEffect(position - transform.position, -dir);

        return true;
    }

    public IEnumerator Slow(float slowedMovementSpeed, float duration)
    {
        float previous = CurMovementSpeed;
        CurMovementSpeed = slowedMovementSpeed;
        yield return new WaitForSeconds(duration);
        CurMovementSpeed = previous;
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_RagdollEffect(Vector3 position)
    {
        ActivateRagdoll(true);

        foreach (var rb in ragdollRigidbodys)
        {
            rb.AddExplosionForce(35f, position, 10f, 5f, ForceMode.Impulse);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayDamageEffect(Vector3 relativePosition, Vector3 direction)
    {
        if (hitEffectPrefab != null)
        {
            var hitPosition = transform.position + relativePosition;
            var hitRotation = Quaternion.LookRotation(direction);
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, hitRotation);

            Destroy(effect, 2.0f);
        }

        AudioManager.instance.PlaySfx(Sfxs.Hit);
    }

    /// <summary>
    /// MonsterSpawner가 target으로 들어가있을때 외부에서 지우기 위해
    /// </summary>
    public void ClearTargetList()
    {
        target = null;
        targets.Clear();
        targetBridges.Clear();
    }

    protected virtual void OnWave()
    {
        CurDetectorRadius = info.DetectAreaWave;
    }

    protected virtual void OnDead()
    {
        ActivateRagdoll(true);
        GetComponent<NetworkTransform>().enabled = false;
        if (NetworkGameManager.Instance != null)
            NetworkGameManager.Instance.monsterSpawner.SpawnedNum--;
        AnalyticsManager.analyticsZombieKill(key);
    }

    private void ActivateRagdoll(bool active)
    {

        animator.enabled = !active;

        foreach (Rigidbody rb in ragdollRigidbodys)
        {
            rb.isKinematic = !active;
        }

        foreach (BoxCollider col in ragdollBoxCollider)
        {
            col.enabled = active;
        }

        foreach (CapsuleCollider col in ragdollCapsuleCollider)
        {
            col.enabled = active;
        }

        foreach (SphereCollider col in ragdollSphereCollider)
        {
            col.enabled = active;
        }
    }
}
