using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNetworkBehaviour : NetworkBehaviour
{
    /// <summary>
    /// The networked amount of health that monster has
    /// </summary>
    [Networked, Tooltip("The networked amount of health that monster has")]
    public float Health { get; private set; } = 100;

    [Networked, Tooltip("The networked amount of health that monster has")]
    public float MovementSpeed { get; set; } = 0.0f;

    [Networked]
    public bool IsAttack { get; set; } = false;

    [Tooltip("Reference to the enemy's FSM.")]
    public MonsterFSM FSM;

    [Tooltip("Reference to the NavMeshAgent used to determine where the enemy should move to.")]
    public NavMeshAgent AIPathing;

    public Transform target;

    public Animator animator;

    public override void Spawned()
    {
        base.Spawned();
        target = FindAnyObjectByType<MonsterTest>().transform;
    }

    protected virtual void Update()
    {
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
        animator.SetBool("IsAttack", IsAttack);
        AIPathing.speed = MovementSpeed;
    }

    public virtual void PlayerDetectedListnerByPlayer()
    {
    }
}
