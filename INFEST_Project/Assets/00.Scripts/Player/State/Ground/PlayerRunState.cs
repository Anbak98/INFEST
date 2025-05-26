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
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        player.animationController.isFiring = data.isFiring;
        PlayerRun(data);
        //controller.ApplyGravity();  // ม฿ทย

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
