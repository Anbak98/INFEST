using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // 가장 먼저 시작
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   // Ground 진입
        //Debug.Log("Idle상태 진입");

        // Ground && Idle
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
        base.PhysicsUpdate(data);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        //controller.ApplyGravity();  // 중력

        // Idle 또한 X, Z 방향으로 회전할 수 있어야함
        player.animationController.LookDirection = data.lookDelta;



        if (controller.IsGrounded() && data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        else if ((controller.IsGrounded()) && data.isJumping)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        else if (controller.IsGrounded() && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        // 일단 샷건(isShotgunOnFiring)은 미작성
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        else if (controller.IsGrounded() && data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else if (controller.IsGrounded() && data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
