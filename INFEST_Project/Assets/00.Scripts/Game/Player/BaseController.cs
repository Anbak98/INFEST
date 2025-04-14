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
        // animator�� �ִ� ���� �߰��ߴ�(1��Ī, 3��Ī ����)
        animator = GetComponent<Animator>();        
    }

    protected virtual void Update()
    {
        stateMachine?.Update();
    }
}
