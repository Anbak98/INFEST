using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitIdleState : PlayerSitState
{
    public PlayerSitIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();
        Debug.Log("SitIdle���� ����");

        // Sit && Idle
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        controller.ApplyGravity();  // �߷�

        // �Է°��� �ִٸ� MoveState�� ��ȯ
        if (stateMachine.InputHandler.GetMoveInput() != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.WaddleState);
            return;
        }
        if (!stateMachine.InputHandler.GetIsSitting())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    // SitIdle���� ����� �̺�Ʈ�� 
    protected override void AddInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.fire).started += OnSitAttackStarted;
        inputManager.GetInput(EPlayerInput.reload).started += OnSitReloadStarted;

    }
    protected override void RemoveInputActionsCallbacks()
    {
        //inputManager.GetInput(EPlayerInput.move).canceled += OnWaddleStarted;
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
