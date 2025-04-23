using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        if (controller.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
