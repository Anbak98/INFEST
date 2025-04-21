using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // ���� ���� ����
    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   // Ground ����
        Debug.Log("Idle���� ����");

        // Ground && Idle
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Update()
    {
        base.Update();
        controller.ApplyGravity();  // �߷�

        // �Է°��� �ִٸ� MoveState�� ��ȯ
        if (stateMachine.InputHandler.GetMoveInput() != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        //if (stateMachine.InputHandler.GetIsJumping())
        //{
        //    stateMachine.ChangeState(stateMachine.JumpState);
        //}

        //if (stateMachine.InputHandler.GetIsReloading())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
        //// ���⸦ ���� ���¿��� �Է��� ������ Reload ���·�
        //if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsReloading())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
    }

    // Idle���� ����� �̺�Ʈ��
    protected override void AddInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.move).started += OnMoveStarted;
        inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;
        inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started += OnReload;
        inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    protected override void RemoveInputActionsCallbacks()
    {
        //inputManager.GetInput(EPlayerInput.move).canceled += OnMoveStarted;
        //inputManager.GetInput(EPlayerInput.run).canceled += OnRunStarted;
        //inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;
        //inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;
        //inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
        inputManager.GetInput(EPlayerInput.move).started -= OnMoveStarted;
        inputManager.GetInput(EPlayerInput.run).started -= OnRunStarted;
        inputManager.GetInput(EPlayerInput.fire).started -= OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started -= OnReload;
        inputManager.GetInput(EPlayerInput.sit).started -= OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started -= OnJumpStarted;

    }



    // 
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }
    protected override void OnAttack(InputAction.CallbackContext context)
    {
        base.OnAttack(context);
        stateMachine.ChangeState(stateMachine.AttackState);
    }
    protected override void OnReload(InputAction.CallbackContext context)
    {
        base.OnReload(context);
        stateMachine.ChangeState(stateMachine.ReloadState);

    }
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }
    protected override void OnSitStarted(InputAction.CallbackContext context)
    {
        base.OnSitStarted(context);
        stateMachine.ChangeState(stateMachine.SitIdleState);
    }


}
