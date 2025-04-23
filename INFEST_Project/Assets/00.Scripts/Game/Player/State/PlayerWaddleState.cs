using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaddleState : PlayerSitState
{
    public PlayerWaddleState(PlayerController controller, PlayerStateMachine stateMachine, InputManager inputManager) : base(controller, stateMachine, inputManager)
    {
    }
    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 1; // �ȴ� �ӵ��� 0.5��
        Debug.Log("Waddle���� ����");
        base.Enter();


        /// blend tree �ִϸ��̼ǿ� ����
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }
    public override void Exit()
    {
        base.Exit();
        // ���� �ʱ�ȭ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = data.direction;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        // �÷��̾� �̵�
        PlayerWaddle();
        controller.ApplyGravity();  // �߷�


        if (!data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void AddInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.fire).started += OnSitAttackStarted;
        inputManager.GetInput(EPlayerInput.reload).started += OnSitReloadStarted;

    }
    protected override void RemoveInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.fire).started -= OnSitAttackStarted;
        inputManager.GetInput(EPlayerInput.reload).started -= OnSitReloadStarted;
    }

    protected override void OnSitAttackStarted(InputAction.CallbackContext context)
    {
        base.OnAttack(context);
        stateMachine.ChangeState(stateMachine.SitAttackState);
    }
    protected override void OnSitReloadStarted(InputAction.CallbackContext context)
    {
        base.OnAttack(context);
        stateMachine.ChangeState(stateMachine.SitReloadState);
    }
}
