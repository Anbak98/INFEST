using Fusion;
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
    public MonsterInfo info {get; private set;}

    [Header("Monster Number depends on Data Table")]
    public int key = -1; 

    [Header("Monster Control Helper")]
    [Tooltip("Reference to the NavMeshAgent used to determine where the enemy should move to.")]
    public NavMeshAgent AIPathing;

    [Header("Monster Render Helper")]
    public Animator animator;
    public GameObject hitEffectPrefab;
    public AudioSource hitSound;
    public AudioClip hitSoundClip;

    [Header("Monster Status")]
    public SphereCollider PlayerDetectorCollider;

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

    [ReadOnly] public Transform target;
    [ReadOnly, SerializeField] private List<Transform> targets = new();
    private Dictionary<Transform, TargetableFromMonster> targetBridges = new();

    public virtual void OnWave(Transform target)
    {
        this.target = target;
    }

    public virtual void OnDead()
    {
    }

    public override void Spawned()
    { 
        info = DataManager.Instance.GetByKey<MonsterInfo>(key);

        BaseHealth = info.MinHealth * Runner.SessionInfo.PlayerCount;
        BaseDamage = info.MinAtk;
        BaseDef = info.MinDef;

        CurHealth = BaseHealth;
        CurDamage = BaseDamage;
        CurDef = BaseDef;

        CurMovementSpeed = info.SpeedMove;

        PlayerDetectorCollider.radius = info.DetectAreaNormal;

        AIPathing.enabled = true;
        AIPathing.speed = info.SpeedMove;
    }

    public void Move()
    {
        AIPathing.SetDestination(target.position);
    }

    public bool IsLookPlayer()
    {
        foreach(var _target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(dirToTarget, transform.forward.normalized);
            if( dot > Mathf.Cos(30f * Mathf.Deg2Rad))
            {
                target = _target;
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

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TryAddTarget(Transform target)
    {
        if(!targets.Contains(target))
        {
            if(target.TryGetComponent<TargetableFromMonster>(out TargetableFromMonster bridge))
            {
                targets.Add(target);
                targetBridges.Add(target, bridge);
            }
        }
    }

    public void TryRemoveTarget(Transform target)
    {
        if (this.target == target)
        {
            target = null;
        }

        if (targets.Contains(target))
        {
            targets.Remove(target);
            targetBridges.Remove(target);
        }
    }

    public void TryAttackTarget(int damage)
    {
        if(targetBridges.TryGetValue(target, out TargetableFromMonster bridge))
        {
            bridge.ApplyDamage(key, damage);
        }
    }
    public void TryAttackTarget(Transform target, int damage)
    {
        if (targetBridges.TryGetValue(target, out TargetableFromMonster bridge))
        {
            bridge.ApplyDamage(key, damage);
        }
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
        Debug.Log("HI");
        if (isCritical)
            Debug.Log("HeadShot");
        if (CurHealth <= 0f)
            return false;

        CurHealth -= (int)damage;

        if (CurHealth <= 0f)
        {
            CurHealth = 0;
            IsDead = true;
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
    private void RPC_PlayDamageEffect(Vector3 relativePosition, Vector3 direction)
    {
        if (hitEffectPrefab != null)
        {
            var hitPosition = transform.position + relativePosition;
            var hitRotation = Quaternion.LookRotation(direction);
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, hitRotation);

            Destroy(effect, 2.0f);
        }

        if (hitSound != null && hitSoundClip != null)
        {
            hitSound.PlayOneShot(hitSoundClip);
        }
    }

    /// <summary>
    /// MonsterSpawner가 target으로 들어가있을때 외부에서 지우기 위해
    /// </summary>
    public void ClearTargetList()
    {
        targets.Clear();
    }

}
