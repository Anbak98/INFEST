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

    [Networked, OnChangedRender(nameof(OnChangedMovementSpeed))]
    public float CurMovementSpeed { get; set; }
    [Networked, OnChangedRender(nameof(OnChangedDetectorRadiusSpeed))]
    public float CurDetectorRadius { get; private set; }
    [Networked] public int CurHealth { get; private set; } = -1;
    public int CurDamage { get; private set; }
    public int CurDef { get; private set; }
    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked] public NetworkBool IsDead { get; set; } = false;


    [field: ReadOnly] public Transform target { get; private set;}
    private List<Transform> targets = new();
    private Dictionary<Transform, PlayerMethodFromMonster> targetBridges = new();


    public override void Spawned()
    { 
        info = DataManager.Instance.GetByKey<MonsterInfo>(key);

        float userCount = Runner.SessionInfo.PlayerCount / 4;

        CurHealth = (int)(info.MinHealth * userCount);
        CurMovementSpeed = info.SpeedMove;
        CurDamage = info.MinAtk;
        CurDef = info.MinDef;

        PlayerDetectorCollider.radius = info.DetectAreaNormal;

        AIPathing.enabled = true;
        AIPathing.speed = info.SpeedMove;
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
        if (targets.Count > 1)
        {
            Transform newTarget;
            do
            {
                newTarget = targets[Random.Range(0, targets.Count)];
            } while (target != newTarget);
            target = newTarget;
        }
        else if (targets.Count == 1)
        {
            target = targets[0];
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
            if(target.TryGetComponent<PlayerMethodFromMonster>(out PlayerMethodFromMonster bridge))
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
        if(targetBridges.TryGetValue(target, out PlayerMethodFromMonster bridge))
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

    public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
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
}
