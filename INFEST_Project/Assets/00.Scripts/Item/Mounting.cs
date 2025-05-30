using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Mounting : TargetableFromMonster
{
    public Shield obj;

    private Animator animator;
    private int _isStopHash = Animator.StringToHash("IsComplete");


    private void Awake()
    {
        CurHealth = 1000;
        animator = GetComponent<Animator>();
    }

    public override void ApplyDamage(MonsterNetworkBehaviour attacker, int amount)
    {
        CurHealth -= amount;
        if (CurHealth <= 0)
        {
            CurHealth = 0;
            attacker.TryRemoveTarget(transform);
            Runner.Despawn(Object);
        }
    }

    public void Init(Shield shield)
    {
        obj = shield;
        CurHealth = obj.instance.data.Effect;
    }

    public void StopAnimation()
    {
        animator.SetTrigger(_isStopHash);
    }
}
