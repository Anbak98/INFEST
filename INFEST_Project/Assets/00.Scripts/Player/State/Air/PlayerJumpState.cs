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
        //Debug.Log("Jump상태 진입");
        base.Enter();

        PlayerJump();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isJumping = data.isJumping; 
        PlayerMove(data);

        player.animationController.isFiring = data.isFiring;

        if (data.isFiring)
            PlayerFire(data);
        controller.ApplyGravity();  // 중력
        if (controller.GetVerticalVelocity() <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
