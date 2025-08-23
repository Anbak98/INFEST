using Fusion;
using Infest.Monster;
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
    private IMonsterStatHandler statHandler;
    private IMonsterMoveHandler moveHandler;
    private IMonsterLookHandler lookHandler;

    public void MoveToTarget() => moveHandler.Move();
    public bool MoveToRandomPositionAndCheck(float minDistance, float maxDistance, float radius) => moveHandler.MoveRandomly(minDistance, maxDistance, radius);

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

    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnDead))] public NetworkBool IsDead { get; set; } = false;

    public override void Spawned()
    {
        AudioManager.instance.PlaySfx(Sfxs.ZombieSpawn);

        int playerCount = Runner.SessionInfo.PlayerCount - 1 > 0 ? Runner.SessionInfo.PlayerCount - 1 : 0;
        int elapsedTime = (int)(Runner.SimulationTime / 300);

        statHandler.Init(key, playerCount, elapsedTime);

        AIPathing.enabled = true;
        AIPathing.speed = info.SpeedMove;
        ActivateRagdoll(false);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
    }

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
