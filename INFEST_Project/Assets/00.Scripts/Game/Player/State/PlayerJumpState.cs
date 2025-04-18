using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    public override void Update()
    {
        // 점프(y방향 속도가 0보다 클때까지는 JumpState)

        // 땅에 닿을 때까지(IsGrounded == true) 중력을 받는다

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

}
