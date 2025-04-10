using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected PlayerController controller;
    protected PlayerStateMachine stateMachine;

    public BaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// 상태 진입 시 호출
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// 상태 종료 시 호출
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 입력 처리
    /// </summary>
    public virtual void HandleInput(NetworkInputData input) { }

    /// <summary>
    /// 매 FixedUpdateNetwork마다 호출됨
    /// </summary>
    public virtual void UpdateLogic() { }

    /// <summary>
    /// 필요하다면 상태별로 물리 계산도 가능
    /// </summary>
    public virtual void UpdatePhysics() { }
}
