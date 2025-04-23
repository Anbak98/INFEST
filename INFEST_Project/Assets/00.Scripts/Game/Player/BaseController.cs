using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseController : NetworkBehaviour
{
    public StateMachine stateMachine;
    [Networked] protected TickTimer delay { get; set; }

    public virtual void Awake()
    {
    }

    public virtual void Update()
    {
        stateMachine?.OnUpdate();
    }

    public virtual bool IsGrounded() => true;
    public virtual bool IsJumpInput() => false;
    public virtual bool IsSitInput() => false;

    //public virtual bool IsFiring() => false;
    //public virtual bool IsShotgunFiring() => false;
    public abstract void HandleMovement(NetworkInputData data);
    public abstract void ApplyGravity();
    public abstract void StartJump();
    public abstract void StartFire(NetworkInputData data);
    public abstract void StartReload(NetworkInputData data);
    public virtual float GetVerticalVelocity() => 0f;


    public abstract void StartSit();
    public abstract void StartStand();
}
