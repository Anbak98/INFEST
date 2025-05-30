using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput에서 받은 정보를 바탕으로 상태를 업데이트(유지 혹은 변화)
/// ThirdPersonRoot의 애니메이션을 기준으로 상태를 나누었다(FirstPersonRoot은 상태와 상관없이 애니메이션이 실행된다)
/// 단, AttackState는 FirstPersonRoot 기준이다(서서 쏘기, 앉아서 쏘기를 구분하지 않기 때문)
/// </summary>
public class PlayerStateMachine
{
    // player의 입력정보를 가져오기만 한다
    public Player Player { get; }

    // 공통
    public PlayerAttackState AttackState { get; private set; }
    public PlayerAimState AimState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }

    // Ground
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerReloadState ReloadState { get; private set; }

    // Air
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    // Sit
    public PlayerSitIdleState SitIdleState { get; private set; }
    public PlayerWaddleState WaddleState { get; private set; }
    // 현재 상태를 가지고 있어야한다
    public IState currentState;

    // 확장성을 고려하면 BaseController를 선언하는게 맞지만
    // 사용하는 곳에서 ((PlayerController) controller).PlayerAnimation 처럼 쓸때마다 형변환해야하는 문제가 생길 수 있음
    // PlayerStateMachine에 PlayerController를 바로 선언할 수도 있지만 이때는 확장성이 줄어든다는 단점이 있다
    protected PlayerController controller;


    // 현재의 상태를 종료하고 새로운 상태를 실행
    public void ChangeState(IState newState)
    {
        if (currentState?.GetType() == newState?.GetType()) return;

        /// 가장 처음으로 들어가는 State는 IdleState
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }


    // 생명주기함수 아니다
    public void OnUpdate(NetworkInputData data)
    {
        currentState?.OnUpdate(data);
    }

    public void PhysicsUpdate(NetworkInputData data)
    {
        currentState?.PhysicsUpdate(data);
    }


    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;
        this.controller = controller;

        // 수정사항
        IdleState = new PlayerIdleState(controller, this);
        MoveState = new PlayerMoveState(controller, this);
        RunState = new PlayerRunState(controller, this);

        AttackState = new PlayerAttackState(controller, this);

        AimState = new PlayerAimState(controller, this);
        ReloadState = new PlayerReloadState(controller, this);

        JumpState = new PlayerJumpState(controller, this);
        FallState = new PlayerFallState(controller, this);

        SitIdleState = new PlayerSitIdleState(controller, this);
        WaddleState = new PlayerWaddleState(controller, this);
        DeadState = new PlayerDeadState(controller, this);


        // 처음에는 IdleState시작
        currentState = IdleState;
    }

    public bool TryGetNextState(NetworkInputData data, out IState nextState)
    {
        nextState = null;
        if (currentState is PlayerIdleState)
        {
            if (data.direction != Vector3.zero)
            {
                nextState = MoveState;
            }
            else if (data.isRunning && controller.IsGrounded())
            {
                nextState = RunState;
            }
            else if (data.isJumping && controller.IsGrounded())
            {
                nextState = JumpState;
            }
            else if (data.isSitting && controller.IsGrounded())
            {
                nextState = SitIdleState;
            }
            else if (data.isZooming)
            {
                nextState = AimState;
            }
        }
        else if (currentState is PlayerMoveState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
            }
            else if (data.isJumping && controller.IsGrounded())
            {
                nextState = JumpState;
            }
            else if (data.isRunning && controller.IsGrounded())
            {
                nextState = RunState;
            }
            else if (data.isSitting && data.direction != Vector3.zero)
            {
                nextState = WaddleState;
            }
            else if (data.isZooming && controller.IsGrounded())
            {
                nextState = AimState;
            }
        }
        else if (currentState is PlayerRunState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
            }
            else if (!data.isRunning && controller.IsGrounded())
            {
                nextState = MoveState;
            }
            else if (data.isJumping && controller.IsGrounded())
            {
                nextState = JumpState;
            }
        }
        else if (currentState is PlayerJumpState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
            }
            else if (data.direction != Vector3.zero)
            {
                nextState = MoveState;
            }
            else if (data.isRunning)
            {
                nextState = RunState;
            }
        }
        else if (currentState is PlayerFallState)
        {
            if (controller.IsGrounded() && !data.isSitting)
            {
                nextState = IdleState;
            }
            else if (controller.IsGrounded() && data.isSitting)
            {
                nextState = SitIdleState;
            }
        }
        else if (currentState is PlayerSitIdleState)
        {
            if (!data.isSitting && data.direction == Vector3.zero)
            {
                nextState = IdleState;
            }
            else if (data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero)
            {
                nextState = WaddleState;
            }
            else if (data.isJumping && controller.IsGrounded())
            {
                nextState = JumpState;
            }
            else if (data.isZooming && controller.IsGrounded())
            {
                nextState = AimState;
            }
        }
        else if (currentState is PlayerWaddleState)
        {
            if (!data.isSitting && data.direction == Vector3.zero)
            {
                nextState = IdleState;
            }
            else if (!data.isSitting && data.direction != Vector3.zero)
            {
                nextState = MoveState;
            }
            else if (data.isSitting && data.direction == Vector3.zero)
            {
                nextState = SitIdleState;
            }
            else if (data.isJumping && controller.IsGrounded())
            {
                nextState = JumpState;
            }
        }
        else if (currentState is PlayerAimState)
        {
            if (!data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero && !data.isZooming)
            {
                nextState = IdleState;
            }
            else if (!data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero && !data.isZooming)
            {
                nextState = MoveState;
            }
            else if (data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero && !data.isZooming)
            {
                nextState = SitIdleState;
            }
            else if (data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero && !data.isZooming)
            {
                nextState = WaddleState;
            }
        }
        return nextState != null;   // false: 이전상태 그대로 유지
    }
}

