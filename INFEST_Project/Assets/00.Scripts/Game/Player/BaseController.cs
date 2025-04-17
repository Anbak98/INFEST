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
    public T GetStateMachine<T>() where T : StateMachine => stateMachine as T;
    

    // 상태 관련 함수
    public abstract void PlayFireAnim();
    public virtual bool HasMoveInput() => false;
    public virtual bool IsGrounded() => true;
    public virtual bool IsJumpInput() => false;
    public virtual bool IsFiring() => false;
    public virtual bool IsShotgunFiring() => false;
    public virtual void HandleMovement() { }   // 실제 이동(PlayerInputHandler에서는 값만 저장했다)
    public abstract void ApplyGravity();
    public abstract void StartJump();
    public abstract void HandleFire(bool started);
    public virtual float GetVerticalVelocity() => 0f;
    public virtual Vector3 GetMoveInput() => Vector3.zero;

    // 상태는 여기에서 변화시킨다
    public abstract void ApplyNetworkState(PlayerStatData data);
}
