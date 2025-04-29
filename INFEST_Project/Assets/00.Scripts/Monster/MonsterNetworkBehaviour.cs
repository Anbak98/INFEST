using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNetworkBehaviour : NetworkBehaviour
{            
    //"Name": "PJ_H I",
    //"MonsterType": 0,
    //"MinHealth": 150,
    //"MaxHealth": 250,
    //"HealthPer5Min": 25,
    //"MinAtk": 30,
    //"MaxAtk": 42,
    //"AtkPer5Min": 3,
    //"MinDef": 20,
    //"MaxDef": 28,
    //"DefPer5Min": 2,
    //"SpeedMove": 50,
    //"SpeedAtk": 0.7,
    //"DetectAreaNormal": 5,
    //"DetectAreaWave": 500,
    //"State": 200,
    //"DropGold": 30,
    //"FieldSpawn": true,
    //"LimitSpawnCount": 9999
    public MonsterInfo info;

    public int key = -1;

    [Networked, Tooltip("The networked amount of health that monster has")]
    public float CurrentHealth { get; private set; } = -1;

    [Networked, Tooltip("The networked amount of health that monster has")]
    public float MovementSpeed { get; set; } = -1;

    [Networked] public NetworkBool IsAttack { get; set; } = false;
    [Networked] public NetworkBool IsDead { get; set; } = false;

    [Tooltip("Reference to the enemy's FSM.")]
    public MonsterFSM FSM;

    [Tooltip("Reference to the NavMeshAgent used to determine where the enemy should move to.")]
    public NavMeshAgent AIPathing;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public PlayerStatHandler targetStatHandler;

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

        info = DataManager.Instance.GetByKey<MonsterInfo>(key);

        CurrentHealth = Random.Range(info.MinHealth, info.MaxHealth);
        MovementSpeed = info.SpeedMove;
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
