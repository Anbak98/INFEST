using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundState
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
        // ���� �ʱ�ȭ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void Update()
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        // �÷��̾� �̵�
        PlayerMove();
        controller.ApplyGravity();  // �߷�


        //// �̵� �Է��� ������ Idle ���·�
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
        //// run �����ٸ� �޸���(������ 1�� ������ bool���� �����ϸ� �ٽ� ������ ������ ��� �޸��� ���)
        //else if (stateMachine.InputHandler.GetIsRunning())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
        //// ���⸦ ���� ���¿��� ���ݹ�ư �����ٸ� ���ݻ���
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsFiring())
        //{
        //}
        //// ���⸦ ���� ���¿��� �Է��� ������ Reload ���·�
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsReloading())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
        //// jump �����ٸ� ����
        //else if (stateMachine.InputHandler.GetIsJumping())
        //{
        //    stateMachine.ChangeState(stateMachine.JumpState);
        //}
        //// �ɱ�� ��ȯ
        //else if (stateMachine.InputHandler.GetIsSitting())
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }

    // Move���� ����� �̺�Ʈ�� 
    protected override void AddInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;    
        inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started += OnReload;
        inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    protected override void RemoveInputActionsCallbacks()
    {
        //inputManager.GetInput(EPlayerInput.run).canceled += OnRunStarted;
        //inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;
        //inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;
        //inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
        inputManager.GetInput(EPlayerInput.run).started -= OnRunStarted;
        inputManager.GetInput(EPlayerInput.fire).started -= OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started -= OnReload;
        inputManager.GetInput(EPlayerInput.sit).started -= OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started -= OnJumpStarted;
    }





    // Move������ Idle, Run, Attack, Reload, Jump, Sit���� ������ȯ �����ϴ�
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
