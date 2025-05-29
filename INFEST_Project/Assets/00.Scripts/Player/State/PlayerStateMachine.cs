using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// ThirdPersonRoot�� �ִϸ��̼��� �������� ���¸� ��������(FirstPersonRoot�� ���¿� ������� �ִϸ��̼��� ����ȴ�)
/// ��, AttackState�� FirstPersonRoot �����̴�(���� ���, �ɾƼ� ��⸦ �������� �ʱ� ����)
/// </summary>
public class PlayerStateMachine
{
    // player�� �Է������� �������⸸ �Ѵ�
    public Player Player { get; }

    // ����
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
    // ���� ���¸� ������ �־���Ѵ�
    public IState currentState;

    // Ȯ�强�� ����ϸ� BaseController�� �����ϴ°� ������
    // ����ϴ� ������ ((PlayerController) controller).PlayerAnimation ó�� �������� ����ȯ�ؾ��ϴ� ������ ���� �� ����
    // PlayerStateMachine�� PlayerController�� �ٷ� ������ ���� ������ �̶��� Ȯ�强�� �پ��ٴ� ������ �ִ�
    protected PlayerController controller;


    // ������ ���¸� �����ϰ� ���ο� ���¸� ����
    public void ChangeState(IState newState)
    {
        if (currentState?.GetType() == newState?.GetType()) return;

        /// ���� ó������ ���� State�� IdleState
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }


    // �����ֱ��Լ� �ƴϴ�
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

        // ��������
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

        // ó������ IdleState����
        currentState = IdleState;

        //currentState = FallState;         // ó���� ���ִ� ���¿��� ��������
    }

    // ������ �ð��� ������ ���⿡ ��� ���¸� ���Ƴְ�, ���߿� �и��Ѵ�
    // �̷��� �ϸ� ���¿��� �ٸ� ���¸� ��ø�ؼ� ���� ���� ����
    public bool TryGetNextState(NetworkInputData data, out IState nextState)
    {
        nextState = null;
        // Idle
        if (currentState is PlayerIdleState idleState)
        {
            if (data.direction != Vector3.zero)
            {
                nextState = MoveState;
                return true;
            }
            else if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
            else if (controller.IsGrounded() && data.isSitting)
            {
                nextState = SitIdleState;
                return true;
            }
            else if (data.isZooming)
            {
                nextState = AimState;
                return true;
            }
            // Attack�� ��� ���¿��� �����ϴϱ� DeadState�� ���������� ���� ó���ϴ� ���� �´�
            //if (Player.Weapons != null && data.isFiring)
            //{
            //    nextState = AttackState;
            //    controller.StartFire(data); // ������ ���� ������ ���� 1�� �̸� ȣ��
            //    return true;
            //}
        }
        // Move
        else if (currentState is PlayerMoveState moveState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
                return true;
            }
            else if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
            else if (data.isRunning)
            {
                nextState = RunState;
                return true;
            }
            else if (data.isZooming)
            {
                nextState = AimState;
                return true;
            }
        }
        // Run
        else if (currentState is PlayerRunState runState)
        {
            if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
            else if (!data.isRunning)
            {
                nextState = MoveState;
                return true;
            }
        }
        // Jump
        else if (currentState is PlayerJumpState jumpState)
        {
            if (data.direction == Vector3.zero)
            {
                nextState = IdleState;
                return true;
            }
            else if (data.direction != Vector3.zero)
            {
                nextState = MoveState;
                return true;
            }
            else if (data.isRunning)
            {
                nextState = RunState;
                return true;
            }
        }
        // Fall
        else if (currentState is PlayerFallState fallState)
        {
            if (controller.IsGrounded() && !data.isSitting)
            {
                nextState = IdleState;
                return true;
            }
            else if (controller.IsGrounded() && data.isSitting)
            {
                nextState = SitIdleState;
                return true;
            }
        }
        // SitIdle
        else if (currentState is PlayerSitIdleState sitIdleState)
        {
            if (controller.IsGrounded() && !data.isSitting)
            {
                nextState = IdleState;
                return true;
            }
            else if (controller.IsGrounded() && data.isSitting && data.direction != Vector3.zero)
            {
                nextState = WaddleState;
                return true;
            }

            else if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
            else if (data.isZooming)
            {
                nextState = AimState;
                return true;
            }
        }
        // Waddle
        else if (currentState is PlayerWaddleState waddleState)
        {
            if (controller.IsGrounded() && data.direction == Vector3.zero)
            {
                nextState = SitIdleState;
                return true;
            }
            else if (controller.IsGrounded() && data.isJumping)
            {
                nextState = JumpState;
                return true;
            }
        }
        // Aim
        else if (currentState is PlayerAimState aimState)
        {
            if (!data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero)
            {
                nextState = IdleState;
                return true;
            }
            else if (!data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero)
            {
                nextState = MoveState;
                return true;
            }
            else if (data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero)
            {
                nextState = SitIdleState;
                return true;
            }
            else if (data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero)
            {
                nextState = WaddleState;
                return true;
            }
        }


        return false;   // Idle
    }
}

