using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 서 있는 상태
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }


    public override void Enter()
    {
        Debug.Log("Ground상태 진입");
        base.Enter();


        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // 상단의 layer로 나간다
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void OnUpdate()
    {
    }
    public override void PhysicsUpdate()
    {
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // 기존에 입력이 없었다면 그냥 리턴
        if (moveInput == Vector2.zero) return;

        // 방향키 이동을 해제했다면 Idle로 바꾼다
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
