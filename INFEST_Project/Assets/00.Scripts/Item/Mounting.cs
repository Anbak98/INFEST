using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Mounting : NetworkBehaviour
{
    [Networked] public float curDurability { get; set; }

    public Shield obj;

    private Animator animator;
    private int _isStopHash = Animator.StringToHash("IsComplete");


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        curDurability -= amount;
        if (curDurability <= 0)
        {
            curDurability = 0;
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
