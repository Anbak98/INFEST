using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 2;
        Debug.Log("Move상태 진입");
        base.Enter();
        /// blend tree 애니메이션에 적용
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }
    public override void Exit()
    {
        base.Exit();
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }


    public override void Update()
    {

    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }


}
