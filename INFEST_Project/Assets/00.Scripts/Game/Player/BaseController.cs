using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseController : NetworkBehaviour
{
    public StateMachine stateMachine;

    public virtual void Awake()
    {
    }

    public virtual void Update()
    {
    }

    // 쓸때마다 null 나오는데...?
    //public T GetStateMachine<T>() where T : StateMachine => stateMachine as T;


    //// 상태 관련 함수(Local과 Remote 따로 정의)
    //public abstract void PlayFireAnim();
    //public virtual bool HasMoveInput() => false;
    //public virtual bool IsGrounded() => true;
    //public virtual bool IsJumpInput() => false;
    //public virtual bool IsFiring() => false;
    //public virtual bool IsShotgunFiring() => false;
    public abstract void HandleMovement();
    public abstract void ApplyGravity();
    //public abstract void StartJump();
    //public abstract void HandleFire(bool started);
    //public virtual float GetVerticalVelocity() => 0f;
    //public virtual Vector3 GetMoveInput() => Vector3.zero;
}
