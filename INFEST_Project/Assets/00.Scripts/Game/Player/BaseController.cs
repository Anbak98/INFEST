using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseController : NetworkBehaviour
{
    public StateMachine stateMachine;
    public Animator animator;

    protected virtual void Awake()
    {
        // animator이 있는 곳에 추가했다(1인칭, 3인칭 각각)
        animator = GetComponent<Animator>();        
    }

    protected virtual void Update()
    {
        stateMachine?.Update();
    }
}
