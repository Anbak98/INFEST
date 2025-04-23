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
        //Debug.Log("Jump");
        base.Enter();
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        PlayerJump();
        // 진입할때 처음 1번만 점프
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        PlayerMove();
        PlayerMove(data);
        controller.ApplyGravity();

        // y속도 0이하면 fallState로


        if (controller.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

}
