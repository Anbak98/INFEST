using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput에서 받은 정보를 바탕으로 상태를 업데이트(유지 혹은 변화)
/// </summary>
public class PlayerStateMachine
{
    public Player Player { get; }

    // player의 입력정보를 가져오기만 한다

    // Ground
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerAttackWalkState AttackWalkState { get; private set; }

    public PlayerAimState AimState { get; private set; }
    public PlayerAimWalkState AimWalkState { get; private set; }
    public PlayerAimAttackState AimAttackState { get; private set; }
    public PlayerAimAttackWalkState AimAttackWalkState { get; private set; }

    public PlayerReloadState ReloadState { get; private set; }

    // Air
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    // Sit
    public PlayerSitIdleState SitIdleState { get; private set; }
    public PlayerSitAttackState SitAttackState { get; private set; }
    public PlayerWaddleState WaddleState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
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
        AttackWalkState = new PlayerAttackWalkState(controller, this);

        AimState = new PlayerAimState(controller, this);
        AimWalkState = new PlayerAimWalkState(controller, this);
        AimAttackState = new PlayerAimAttackState(controller, this);
        AimAttackWalkState = new PlayerAimAttackWalkState(controller, this);

        ReloadState = new PlayerReloadState(controller, this);

        JumpState = new PlayerJumpState(controller, this);
        FallState = new PlayerFallState(controller, this);

        SitIdleState = new PlayerSitIdleState(controller, this);
        WaddleState = new PlayerWaddleState(controller, this);
        SitAttackState = new PlayerSitAttackState(controller, this);
        DeadState = new PlayerDeadState(controller, this);
        //SitReloadState = new PlayerSitReloadState(controller, this);


        // 처음에는 IdleState시작
        currentState = IdleState;

        //currentState = FallState;         // 처음에 떠있는 상태에서 떨어진다
    }

    // 지금은 시간이 없으니 여기에 모든 상태를 몰아넣고, 나중에 분리한다
    // 이렇게 하면 상태에서 다른 상태를 중첩해서 들어가는 일이 없다
    public bool TryGetNextState(NetworkInputData data, out IState nextState)
    {
        nextState = null;

        if (currentState is PlayerIdleState idleState)
        {
            if (data.direction != Vector3.zero)
            {
                nextState = MoveState;
                return true;
            }
            if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
            //if (Player.Weapons != null && data.isFiring)
            //{
            //    Player.animationController.isFiring = data.isFiring;
            //    //PlayerFire(data);
            //    //stateMachine.ChangeState(stateMachine.AttackState);
            //}
            //if (controller.IsGrounded() && data.isZooming)
            //{
            //    //stateMachine.ChangeState(stateMachine.AimState);
            //}
        }
        // MoveState도 동일한 방식으로 처리
        if (currentState is PlayerMoveState moveState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
                return true;
            }
            if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
        }





        return false;
    }
}

