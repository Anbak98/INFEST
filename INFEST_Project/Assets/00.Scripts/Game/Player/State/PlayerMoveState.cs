using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        //stateMachine.StatHandler.MoveSpeedModifier = 2;
        Debug.Log("Move상태 진입");
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        // 방향 초기화
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다        
        player.animationController.MoveDirection = data.direction;

        // 지속적으로 Blend Tree 파라미터 업데이트
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        PlayerMove(data);
        controller.ApplyGravity();  // 중력

        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if (data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if ((stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        if (data.isJumping)
        {
            //if (controller.IsGrounded())
            //{
            stateMachine.ChangeState(stateMachine.JumpState);
            //}
        }
        if (data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
