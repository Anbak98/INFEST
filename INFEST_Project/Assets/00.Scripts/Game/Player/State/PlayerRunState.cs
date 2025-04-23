using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        //stateMachine.StatHandler.MoveSpeedModifier = 6;
        //Debug.Log("Run");
        base.Enter();
        //StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        Vector2 moveInput = data.direction;

        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        PlayerRun(data);
        controller.ApplyGravity();

        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if (!data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }
}
