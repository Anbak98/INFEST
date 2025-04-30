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

        //player.animationController.MoveDirection = data.direction;
        //PlayerMove(data);

        //player.animationController.isFiring = data.isFiring;
        //if (data.isFiring)
        //    PlayerFire(data);

        // 정지한 상태에서 좌우회전
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.MoveDirection = data.direction;

            stateMachine.ChangeState(stateMachine.JumpState);
        }
        // 일단 샷건(isShotgunOnFiring)은 미작성
        if (stateMachine.Player.GetWeapons() != null && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            //player.animationController.isFiring = data.isFiring;
            //PlayerFire(data);
        }
        if (controller.IsGrounded() && data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
        if (controller.IsGrounded() && data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
        //if (stateMachine.Player.GetWeapons() != null && data.isReloading)
        //{
        //    stateMachine.ChangeState(stateMachine.ReloadState);
        //}
        //if (controller.IsGrounded() && data.isRunning)
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
    }
}
