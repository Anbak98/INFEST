using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // ���� ���� ����
    public override void Enter()
    {
        stateMachine.StatHandler.MoveSpeedModifier = 0;
        Debug.Log("Idle���� ����");
        base.Enter();

        // Blend Tree�� �߽� (0, 0)���� �ִϸ��̼��� ��ȯ��
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Update()
    {
        base.Update();
        // �Է°��� �ִٸ� MoveState�� ��ȯ
        if (stateMachine.InputHandler.GetMoveInput() != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
    }



    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {

    }


}
