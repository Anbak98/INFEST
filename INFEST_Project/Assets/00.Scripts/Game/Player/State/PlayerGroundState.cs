using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �� �ִ� ����
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }


    public override void Enter()
    {
        Debug.Log("Ground���� ����");
        base.Enter();


        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������
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
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // ������ �Է��� �����ٸ� �׳� ����
        if (moveInput == Vector2.zero) return;

        // ����Ű �̵��� �����ߴٸ� Idle�� �ٲ۴�
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
