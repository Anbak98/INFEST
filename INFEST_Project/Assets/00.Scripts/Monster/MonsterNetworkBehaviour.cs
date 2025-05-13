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

    [Networked, HideInInspector, Tooltip("The networked amount of health that monster has")]
    public float CurrentHealth { get; private set; } = -1;

    [Networked, HideInInspector] public NetworkBool IsAttack { get; set; } = false;
    [Networked, HideInInspector] public NetworkBool IsDead { get; set; } = false;
    [Networked, HideInInspector] private Vector3 LastHitPosition { get; set; }
    [Networked, HideInInspector] private Vector3 LastHitDirection { get; set; }
    [Networked, HideInInspector] private int HitCount { get; set; }
    [Networked, OnChangedRender(nameof(SetMovementSpeed)), HideInInspector] public float MovementSpeed { get; set; }

    [ReadOnly] public Transform target;
    [HideInInspector] public List<Transform> targets = new();

    private void SetMovementSpeed()
    {
        AIPathing.speed = MovementSpeed;
    }

    public override void Spawned()
    { 
        info = DataManager.Instance.GetByKey<MonsterInfo>(key);

        CurrentHealth = Random.Range(info.MinHealth, info.MaxHealth);
        AIPathing.speed = info.SpeedMove;

        if (PlayerDetectorCollider.radius != info.DetectAreaWave)
            PlayerDetectorCollider.radius = info.DetectAreaNormal;

        AIPathing.enabled = true;
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
    
    public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
        if (CurrentHealth <= 0f)
            return false;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            IsDead = true;
        }

        Vector3 dir = (transform.position - position).normalized;

        // Store relative hit position.
        // Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.
        LastHitPosition = position - transform.position;
        LastHitDirection = -dir;

        RPC_PlayDamageEffect(LastHitPosition, LastHitDirection);

        HitCount++;

        return true;
    }

    public IEnumerator Slow(float slowedMovementSpeed, float duration)
    {
        float previous = MovementSpeed;
        MovementSpeed = slowedMovementSpeed;
        yield return new WaitForSeconds(duration);
        MovementSpeed = previous;
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
