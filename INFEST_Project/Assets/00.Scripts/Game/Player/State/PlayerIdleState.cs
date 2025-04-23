using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        //stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   
        //Debug.Log("Idle");

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
        controller.ApplyGravity();  //

        if (data.direction != Vector3.zero)
        {
            //(stateMachine.InputHandler.GetMoveInput() != Vector3.zero)
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
        if (data.isJumping)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        if (data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
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
