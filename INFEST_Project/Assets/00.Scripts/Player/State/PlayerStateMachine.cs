using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// </summary>
public class PlayerStateMachine
{
    public Player Player { get; }

    // player�� �Է������� �������⸸ �Ѵ�

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


        // ó������ IdleState����
        currentState = IdleState;

        //currentState = FallState;         // ó���� ���ִ� ���¿��� ��������
    }

    // ������ �ð��� ������ ���⿡ ��� ���¸� ���Ƴְ�, ���߿� �и��Ѵ�
    // �̷��� �ϸ� ���¿��� �ٸ� ���¸� ��ø�ؼ� ���� ���� ����
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
        // MoveState�� ������ ������� ó��
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

