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
        Debug.Log("Idle상태 진입");

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
        controller.ApplyGravity();  // 중력

        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if (data.isJumping)
        {
            //Debug.LogFormat($"NetworkCharacterController.IsGrounded: {controller.IsGrounded()}"); 

            // 땅에 떨어지기 전에는 controller.IsGrounded() ==  false 되어 내부 로직 실행
            // 땅에 닿기 전에 JumpState로 바뀐다
            // JumpState로 바뀌자마자 다시 PlayerJump호출하고 
            // 무한반복된다
            if (controller.IsGrounded())
            {
                // 여기에서 
                stateMachine.ChangeState(stateMachine.JumpState);
            }
        }
        if (data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        // 일단 샷건(isShotgunOnFiring)은 미작성
        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if ((stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        if (data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        if (data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
