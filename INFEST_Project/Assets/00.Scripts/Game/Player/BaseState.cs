using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    public void Enter();    // 상태 진입
    public void Exit();     // 상태 끝
    public void HandleInput();  // 새로운 입력값이 들어오면 이벤트를 추가, 삭제
    public void Update();   // 상태 업데이트
    public void PhysicsUpdate();    // 물리 업데이트 
}

/// <summary>
/// 모든 상태의 기반 클래스
/// </summary>
public abstract class BaseState : IState
{
    protected BaseController controller;    // PlayerController, MonsterController
    protected StateMachine stateMachine;    // PlayerStateMachine, MonsterStateMachine

    public BaseState(BaseController controller, StateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// 상태 진입 시 호출
    /// </summary>
    public virtual void Enter() 
    {
    }

    /// <summary>
    /// 상태 종료 시 호출
    /// </summary>
    public virtual void Exit() 
    {
    }

    /// <summary>
    /// 새로운 입력값이 들어오면 이벤트를 추가, 삭제
    /// </summary>
    public virtual void HandleInput() 
    {
    }


    public virtual void Update()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    ///// <summary>
    ///// 매 FixedUpdateNetwork마다 호출됨
    ///// </summary>
    //public virtual void UpdateLogic() { }

    ///// <summary>
    ///// 필요하다면 상태별로 물리 계산도 가능
    ///// </summary>
}
