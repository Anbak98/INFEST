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
        // 일단 숫자대입. 나중에 PlayStatData.RunSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 6;
        Debug.Log("Run상태 진입");
        base.Enter();
        // Run은 Move를 기반으로 해야하는데... Move인 상태를 기반으로 Run 파라미터를 추가입력해야한다
        // 어쩔래?
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void OnUpdate()
    {
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // 지속적으로 Blend Tree 파라미터 업데이트
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        PlayerRun();
        controller.ApplyGravity();  // 중력

        // 이동 입력이 없을 떄 Idle로 바꾸면 Lshift를 누르고 있는 채 점프를 하게 된다
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
        // run 다시 눌렀다면 걷기
        if (!stateMachine.InputHandler.GetIsRunning())
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        // run일때는 점프할 수 없다
        //else if (stateMachine.InputHandler.GetIsJumping())
        //{
        //    stateMachine.ChangeState(stateMachine.JumpState);
        //}
        // run일때는 공격상태로 전환할 수 없다
    }

    // Run에서는 Idle, Move(따로 작성하지 않았는데 나중에 생각하자) 으로 상태전환 가능하다
    protected override void AddInputActionsCallbacks()
    {
    }
    protected override void RemoveInputActionsCallbacks()
    {
    }


}
