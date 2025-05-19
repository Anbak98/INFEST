using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Mounting : TargetableFromMonster
{
    [Networked] public float curDurability { get; set; }

    public Shield obj;

    private Animator animator;
    private int _isStopHash = Animator.StringToHash("IsComplete");


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void ApplyDamage(MonsterNetworkBehaviour attacker, int amount)
    {
        curDurability -= amount;
        CurHealth = (int)curDurability;
        if (curDurability <= 0)
        {
            curDurability = 0;
            CurHealth = 0;
            attacker.TryRemoveTarget(transform);
            Runner.Despawn(Object);
        }
    }

    public void Init(Shield shield)
    {
        obj = shield;
        curDurability = obj.instance.data.Effect;
    }

    public void StopAnimation()
    {
        animator.SetTrigger(_isStopHash);
    }
}
