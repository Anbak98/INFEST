using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.RunSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 8;
        Debug.Log("Run상태 진입");
        base.Enter();
        // Run은 Move를 기반으로 해야하는데... Move인 상태를 기반으로 Run 파라미터를 추가입력해야한다
        // 어쩔래?
        //StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다
        //Vector3 moveInput = data.direction;

        base.OnUpdate(data);

        player.animationController.isFiring = data.isFiring;
        PlayerRun(data);
        //controller.ApplyGravity();  // 중력

        if (!data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.MoveDirection = data.direction;

            stateMachine.ChangeState(stateMachine.JumpState);
        }

    }
}
