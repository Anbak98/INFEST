using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster : NetworkBehaviour
{
    /// <summary>
    /// The networked amount of health that monster has
    /// </summary>
    [Networked, Tooltip("The networked amount of health that monster has")]
    public float Health { get; private set; } = 100;

    [Networked, Tooltip("The networked amount of health that monster has")]
    public float MovementSpeed { get; set; } = 0.0f;

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

    public void Update()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
    }
}
