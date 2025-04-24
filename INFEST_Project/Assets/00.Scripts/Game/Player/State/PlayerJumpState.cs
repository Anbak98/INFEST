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
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // 점프는 한번만
        PlayerJump();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // 플레이어 이동
        PlayerMove(data);
        controller.ApplyGravity();  // 중력

        // y방향 속도가 작으면 fallstate로 바꾸어야한다

        if (controller.GetVerticalVelocity() < 0)
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
