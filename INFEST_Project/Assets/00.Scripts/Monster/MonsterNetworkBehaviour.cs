using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNetworkBehaviour : NetworkBehaviour
{
    [Networked, Tooltip("The networked amount of health that monster has")]
    public float CurrentHealth { get; private set; } = 100;

    [Networked, Tooltip("The networked amount of health that monster has")]
    public float MovementSpeed { get; set; } = 0.0f;

    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked] public NetworkBool IsDead { get; set; } = false;

    [Tooltip("Reference to the enemy's FSM.")]
    public MonsterFSM FSM;

    [Tooltip("Reference to the NavMeshAgent used to determine where the enemy should move to.")]
    public NavMeshAgent AIPathing;

    [HideInInspector]
    public Transform target;

    public Animator animator;
    [Networked]
    private int HitCount { get; set; }
    [Networked]
    private Vector3 LastHitPosition { get; set; }
    [Networked]
    private Vector3 LastHitDirection { get; set; }

    public GameObject hitEffectPrefab;
    public AudioSource hitSound;
    public AudioClip hitSoundClip;

    public override void Spawned()
    {
        base.Spawned();
    }

    protected virtual void Update()
    {
    }

    public virtual void PlayerDetectedListnerByPlayer()
    {
    }

    public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
        if (CurrentHealth <= 0f)
            return false;

        //if (isImmortal)
        //return false;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            IsDead = true;
            Debug.Log("»ç¸Á");
            //_sceneObjects.Gameplay.PlayerKilled(instigator, Object.InputAuthority, weaponType, isCritical);
        }

        // Store relative hit position.
        // Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.
        LastHitPosition = position - transform.position;
        LastHitDirection = -direction;

        RPC_PlayDamageEffect(LastHitPosition, LastHitDirection);

        HitCount++;

        return true;
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
