using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IState
{
    public void Enter();    // ���� ����
    public void Exit();     // ���� ��
    public void HandleInput();  // ���ο� �Է°��� ������ �̺�Ʈ�� �߰�, ����
    public void Update();   // ���� ������Ʈ
    public void PhysicsUpdate();    // ���� ������Ʈ(�߷� ����) 
}


public abstract class PlayerBaseState : IState
{
    protected PlayerController controller;    // PlayerController, �ٸ� npc�� controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler�� �ִ� �����͸� �б⸸ �Ѵ�
    protected Player player;
    // ������ �÷��̾��� ī�޶� ������ �׳� ������
    // ���߿��� �ٸ� ī�޶�(���ؿ� �����)�� �����Ͽ� CameraHandler�� ���� ī�޶� ������ �� ����

    public Transform MainCameraTransform { get; set; }


    // �ӽ�(��������)
    public InputManager inputManager;

    public PlayerBaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        statHandler = stateMachine.Player.statHandler;
        player = stateMachine.Player;
        inputManager = player.Input.GetInputManager();
        stateMachine.Player.playerAnimator = player.playerAnimator;

        MainCameraTransform = Camera.main.transform;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks(); // BaseState�� ���� �� �̺�Ʈ ���
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();  // �̺�Ʈ ����
    }

    // �Է� ���� Ȯ��
    public virtual void HandleInput()
    {
        // StateMachine���� ȣ���ϴ� HandleInput��
        // BaseState��  ReadMovementInput�� ȣ���ϴµ�
        // ReadMovementInput�� ���⿡���� ȣ���Ѵ�
        // �̰� Controller�� HandleMovement�� �� �� ���Ĵ°���
        // �߷µ� �������� ApplyGravity

        ReadMovementInput();    // �̵�
        // �޸���
        // ����
        // ���
        // �ɱ�

    }
    public virtual void Update()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }

    #region ���� ���� �̺�Ʈ#
    // ��� ���¿� ������ �޼���
    // ���� Ŭ�������� ����� �̺�Ʈ�� ���� ���� �ÿ� �߰�
    protected virtual void AddInputActionsCallbacks()
    {
        // ���¿� �������� �� �߰��Ѵ�
        //inputManager.GetInput(EPlayerInput.move).started += OnMovementCanceled; // Ű�� �������� ��
        //inputManager.GetInput(EPlayerInput.move).started += OnMoveStarted; // Ű�� �������� ��
        //inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;    // Ű�� ������ ��
        //inputManager.GetInput(EPlayerInput.look).started += OnLookStarted;

        //inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).started += OnReload;

        //inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;

        //inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    // �̺�Ʈ ����
    protected virtual void RemoveInputActionsCallbacks()
    {
        // ���¸� �������� �� �߰��Ѵ�
        //inputManager.GetInput(EPlayerInput.move).canceled -= OnMoveStarted;
        //inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

        //inputManager.GetInput(EPlayerInput.look).canceled += OnLookStarted;

        //inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;

        //inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;

        //inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
    }

    // ���� ���� ���·� ������ ��쿡 �����Ѵ�
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // ����Ű �̵��� �����ߴٸ� Idle�� �ٲ۴�
    }
    protected virtual void OnMoveStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
    }
    //protected virtual void OnLookStarted(InputAction.CallbackContext context)
    //{
    //    //Vector2 lookDelta = context.ReadValue<Vector2>();
    //}
    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnReload(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnWaddleStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitReloadStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitAttackStarted(InputAction.CallbackContext context)
    {
    }

    #endregion
    #region �ִϸ��̼� ��ü
    // bool �Ķ����
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    }
    // Trigger �ĸ�����
    protected void SetTriggerAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetTrigger(animatorHash);
    }
    // MoveX, MoveZ
    protected void SetAnimationFloat(int animatorHash, float value)
    {
        stateMachine.Player.playerAnimator.SetFloat(animatorHash, value);
    }

    #endregion

    #region �Է°� �б�
    // ��� ���´� �Է°��� �޴´�
    // WASD �Է�
    // �̰� BaseState������ ȣ���ϰ��ִ�
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }
    private void ReadBoolInput()
    {
        stateMachine.InputHandler.GetIsJumping();
        // �̷������� �� ���;��ұ�
    }

    #endregion
    #region �ִϸ��̼��� �ִ� �͵�(�̵�, �޸���, ���, ����, �ɱ�, �ɾƼ� �̵�, ����)�� ���� ����
    protected void PlayerMove()
    {
        // ī�޶��� ȸ������(CameraHandler�� Update���� �ǽð����� ������Ʈ)���� �̵��Ѵ�
        controller.HandleMovement();    // �̵�
        controller.ApplyGravity();  // �߷�
    }
    

    protected void PlayerFire()
    {
        Debug.Log("Fire");
        // �߻���� PlayerController�� �ű��
    }
    protected void PlayerRun()
    {
        Debug.Log("Run");
        // ī�޶��� ȸ������(CameraHandler�� Update���� �ǽð����� ������Ʈ)���� �̵��Ѵ�
        controller.HandleMovement();    // �̵�
        controller.ApplyGravity();  // �߷�

    }
    protected void PlayerJump()
    {
        Debug.Log("Jump");

        // Junp Ű�Է��ϸ� ���ο��� 1���� y�� ���ް� �� �ܴ� ���� ���� ������ �߷¸� �������̴�
        controller.StartJump();
    }
    protected void PlayerSit()
    {
        Debug.Log("Sit");

    }
    // �ɾƼ� �ȱ�
    protected void PlayerWaddle()
    {
        Debug.Log("Waddle");
        // ī�޶��� ȸ������(CameraHandler�� Update���� �ǽð����� ������Ʈ)���� �̵��Ѵ�
        controller.HandleMovement();    // �̵�
        controller.ApplyGravity();  // �߷�

    }
    // ����(�ִϸ��̼��� �ٲٰ�, ī�޶� ���� ����)
    protected void PlayerZoom()
    {
        Debug.Log("Zoom");

    }
    #endregion
}
