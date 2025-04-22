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
        Debug.Log("SitIdle상태 진입");

        // Sit && Idle
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        controller.ApplyGravity();  // 중력

        // 입력값이 있다면 MoveState로 전환
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.WaddleState);
            return;
        }
        if (!data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    // SitIdle에서 사용할 이벤트는 
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
