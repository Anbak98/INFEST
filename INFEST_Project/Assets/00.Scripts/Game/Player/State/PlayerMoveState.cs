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
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 2;
        Debug.Log("Move���� ����");
        base.Enter();
        /// blend tree �ִϸ��̼ǿ� ����
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
