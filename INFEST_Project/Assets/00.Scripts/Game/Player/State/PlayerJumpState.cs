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
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // 점프는 한번만
        PlayerJump();

        // OnUpdate에서 controller.IsGrounded()가 땅에 떨어져 있을때에도 true를 리턴하는 문제가 있어서 옮겼다
        //stateMachine.ChangeState(stateMachine.IdleState);
        // PlayerJump하자마자 땅에 닿기전에 IdleState로 진입하고
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // 플레이어 이동
        PlayerMove(data);
        controller.ApplyGravity();  // 중력

        // y방향 속도가 작으면 fallstate로 바꾸어야한다(이건 일단 나중에

        // 땅에 떨어졌는데 controller.IsGrounded()가 true를 리턴하는 것이 문제다
        // 다른 조건을 추가하여 땅에서 발이 떨어졌을 때 false가 되어야한다
        // 땅에 붙은 다음에는 false가 되는건 왜그런거지???
        //Debug.LogFormat($"NetworkCharacterController.IsGrounded: {controller.IsGrounded()}");
        //if (controller.IsGrounded())
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
        if (controller.GetVerticalVelocity() <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // 점프가 끝나면 이전 상태로 돌아가야한다
    }

}
