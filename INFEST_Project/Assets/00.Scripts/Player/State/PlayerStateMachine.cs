using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// ThirdPersonRoot�� �ִϸ��̼��� �������� ���¸� ��������(FirstPersonRoot�� ���¿� ������� �ִϸ��̼��� ����ȴ�)
/// ��, AttackState�� FirstPersonRoot �����̴�(���� ���, �ɾƼ� ��⸦ �������� �ʱ� ����)
/// </summary>
public enum PlayerStateType
{
    Idle,
    Move,
    Run,
    Jump,
    Fall,
    SitIdle,
    Waddle,
    Aim,
    Dead,
    Attack
}

public class PlayerStateMachine
{
    // player�� �Է������� �������⸸ �Ѵ�
    public Player Player { get; }

    public Dictionary<PlayerStateType, IState> stateMap;
    public PlayerStateType currentStateType;

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

        stateMap = new Dictionary<PlayerStateType, IState>{
                    { PlayerStateType.Idle, IdleState },
                    { PlayerStateType.Move, MoveState },
                    { PlayerStateType.Run, RunState },
                    { PlayerStateType.Jump, JumpState },
                    { PlayerStateType.Fall, FallState },
                    { PlayerStateType.SitIdle, SitIdleState },
                    { PlayerStateType.Waddle, WaddleState },
                    { PlayerStateType.Aim, AimState },
                    { PlayerStateType.Dead, DeadState },
                    { PlayerStateType.Attack, AttackState }
                    };

        // ó������ IdleState����
        currentStateType = PlayerStateType.Idle;
        currentState = stateMap[currentStateType];
    }

    public bool TryGetNextState(NetworkInputData data, out IState nextState)
    {
        nextState = null;
        // Dictionary �˻����ٴ� ���������� ������ �� ��� �ϴ��� ���� �������� ó���ߴ�
        switch (currentStateType)
        {
            case PlayerStateType.Idle:
                if (data.direction != Vector3.zero)
                    nextState = MoveState;
                else if (data.isRunning && controller.IsGrounded())
                    nextState = RunState;
                else if (data.isJumping && controller.IsGrounded())
                    nextState = JumpState;
                else if (data.isSitting && controller.IsGrounded())
                    nextState = SitIdleState;
                else if (data.isZooming)
                    nextState = AimState;
                break;

            case PlayerStateType.Move:
                if (data.direction == Vector3.zero)
                    nextState = IdleState;
                else if (data.isJumping && controller.IsGrounded())
                    nextState = JumpState;
                else if (data.isRunning && controller.IsGrounded())
                    nextState = RunState;
                else if (data.isZooming && controller.IsGrounded())
                    nextState = AimState;
                break;

            case PlayerStateType.Run:
                if (data.direction == Vector3.zero)
                    nextState = IdleState;
                else if (!data.isRunning && controller.IsGrounded())
                    nextState = MoveState;
                else if (data.isJumping && controller.IsGrounded())
                    nextState = JumpState;
                break;

            case PlayerStateType.Jump:
                if (data.direction == Vector3.zero)
                    nextState = IdleState;
                else if (data.direction != Vector3.zero)
                    nextState = MoveState;
                else if (data.isRunning)
                    nextState = RunState;
                break;

            case PlayerStateType.Fall:
                if (controller.IsGrounded() && !data.isSitting)
                    nextState = IdleState;
                else if (controller.IsGrounded() && data.isSitting)
                    nextState = SitIdleState;
                break;

            case PlayerStateType.SitIdle:
                if (!data.isSitting && data.direction == Vector3.zero)
                    nextState = IdleState;
                else if (data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero)
                    nextState = WaddleState;
                else if (data.isJumping && controller.IsGrounded())
                    nextState = JumpState;
                else if (data.isZooming && controller.IsGrounded())
                    nextState = AimState;
                break;

            case PlayerStateType.Waddle:
                if (data.isSitting && data.direction == Vector3.zero)
                    nextState = SitIdleState;
                else if (data.isJumping && controller.IsGrounded())
                    nextState = JumpState;
                break;

            case PlayerStateType.Aim:
                if (!data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero && !data.isZooming)
                    nextState = IdleState;
                else if (!data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero && !data.isZooming)
                    nextState = MoveState;
                else if (data.isSitting && controller.IsGrounded() && data.direction == Vector3.zero && !data.isZooming)
                    nextState = SitIdleState;
                else if (data.isSitting && controller.IsGrounded() && data.direction != Vector3.zero && !data.isZooming)
                    nextState = WaddleState;
                break;
        }
        return nextState != null;   // false: �������� �״�� ����
    }
}

