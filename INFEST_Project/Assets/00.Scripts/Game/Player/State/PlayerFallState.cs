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
        // 지면에서 떨어졌다 && y방향 속도가 0보다 작거나 같으면 Fall

        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // 플레이어 이동
        PlayerMove(data);
        controller.ApplyGravity();  // 중력


        // 땅에 닿을 때까지(IsGrounded == true) 중력을 받는다
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
