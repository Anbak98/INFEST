using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
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
    [Networked] public int CurHealth { get; set; } = -1;
    [field: SerializeField] public int CurDamage { get; set; }
    [field: SerializeField] public int CurDef { get; set; }
    [Networked] public int BaseHealth { get; set; } = -1;
    [field: SerializeField] public int BaseDamage { get; set; }
    [field: SerializeField] public int BaseDef { get; set; }
    [Networked] public int OffsetHealth { get; set; } = -1;
    [field: SerializeField] public int OffsetDamage { get; set; }
    [field: SerializeField] public int OffsetDef { get; set; }
    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnDead))] public NetworkBool IsDead { get; set; } = false;

    public override void Spawned()
    {
        info = DataManager.Instance.GetByKey<MonsterInfo>(key);
        AudioManager.instance.PlaySfx(Sfxs.ZombieSpawn);
        BaseHealth = (int)(info.MinHealth * (1 + info.HPCoef * (Runner.SessionInfo.PlayerCount - 1)));
        BaseDamage = (int)(info.MinAtk * (1 + info.AtkCoef * (Runner.SessionInfo.PlayerCount - 1)));
        BaseDef = (int)(info.MinDef * (1 + info.DefCoef * (Runner.SessionInfo.PlayerCount - 1)));

        OffsetHealth = (int)(info.HealthPer5Min * (Runner.SimulationTime / 300));
        OffsetDamage = (int)(info.AtkPer5Min * (Runner.SimulationTime / 300));
        OffsetDef = (int)(info.DefPer5Min * (Runner.SimulationTime / 300));

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

        if (NetworkGameManager.Instance != null)
            NetworkGameManager.Instance.monsterSpawner.SpawnedNum--;
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

    public void SetTargetRandomly()
    {
        if (targets.Count > 0)
        {
            Transform newTarget;
            newTarget = targets[Random.Range(0, targets.Count)];
            target = newTarget;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        this.target = newTarget;
    }

    public void TryAddTarget(Transform newTarget)
    {
        if (!targets.Contains(newTarget))
        {
            if (newTarget.TryGetComponent(out TargetableFromMonster bridge))
            {
                targets.Add(newTarget);
                targetBridges.Add(newTarget, bridge);
            }
        }
    }

    public void TryRemoveTarget(Transform target)
    {
        if (this.target == target)
        {
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

    private void OnChangedMovementSpeed()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        AIPathing.speed = CurMovementSpeed * 2.8f;
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
    }

    protected virtual void OnDead()
    {
        ActivateRagdoll(true);
        GetComponent<NetworkTransform>().enabled = false;
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
