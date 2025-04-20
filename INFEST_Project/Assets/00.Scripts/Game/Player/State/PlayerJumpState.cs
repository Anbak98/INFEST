using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Jump상태 진입");
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // 점프는 한번만
        PlayerJump();
    }
    public override void Update()
    {
        // 플레이어 이동
        PlayerMove();
        controller.ApplyGravity();  // 중력

        // y방향 속도가 작으면 fallstate로 바꿔야한다
        // 땅에 닿을 때까지(IsGrounded == true) 중력을 받는다        

        if (controller.IsGrounded())
        {            
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // 점프가 끝나면 이전 상태로 돌아가야한다
    }

}
