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
        AddInputActionsCallbacks(); // �̺�Ʈ ���
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

        ReadMovementInput();    // �Է� ���� ��� Ȯ��
    }
    public virtual void Update()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }

    #region ���� ���� �̺�Ʈ#
    // ��� ���¿� ������ �޼���
    // ���� ���� �ÿ� �̺�Ʈ �߰�
    protected virtual void AddInputActionsCallbacks()
    {
        // ���¿� �������� �� �߰��Ѵ�
        inputManager.GetInput(EPlayerInput.move).started += OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;

        inputManager.GetInput(EPlayerInput.look).started += OnLookStarted;

        inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started += OnReload;

        inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;

        inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    // �̺�Ʈ ����
    protected virtual void RemoveInputActionsCallbacks()
    {
        // ���¸� �������� �� �߰��Ѵ�
        inputManager.GetInput(EPlayerInput.move).canceled -= OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

        inputManager.GetInput(EPlayerInput.look).canceled += OnLookStarted;

        inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;

        inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;

        inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
    }
    // Ű�� ������ �� 
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // ���� ���¿��� ����
    }
    // Ű�� �Է����� ��
    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        // ���� ���¿��� ����
    }
    protected virtual void OnLookStarted(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        //MainCameraTransform.UpdateRotate(lookDelta.x, lookDelta.y);
    }
    protected virtual void OnAttack(InputAction.CallbackContext context)
    {

    }
    protected virtual void OnReload(InputAction.CallbackContext context)
    {

    }
    // ���� Ű �Է�
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }


    protected virtual void OnSitStarted(InputAction.CallbackContext context)
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

    // ��� ���´� �Է°��� �޴´�
    // WASD �Է�
    // �̰� BaseState������ ȣ���ϰ��ִ�
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }

    #region �̵�
    protected void PlayerMove()
    {
        //Vector3 movementDir = GetMovementDir();
        //Move(movementDir);
        //Rotate(movementDir);

        // ī�޶��� ȸ������(CameraHandler�� Update���� �ǽð����� ������Ʈ)���� �̵��Ѵ�
        controller.HandleMovement();
        controller.ApplyGravity();
    }
    //private Vector3 GetMovementDir()
    //{
    //    // ���� ī�޶� �ٶ󺸴� ���� = �÷��̾ �ٶ󺸴� ����
    //    Transform mainCameraTr = MainCameraTransform.transform;
    //    Vector3 forward = mainCameraTr.forward;
    //    Vector3 right = mainCameraTr.right;

    //    forward.y = 0;
    //    right.y = 0;

    //    forward.Normalize();
    //    right.Normalize();

    //    return forward * inputHandler.GetMoveInput().y + right * inputHandler.GetMoveInput().x;
    //}
    //private void Move(Vector3 dir)
    //{
    //    int movementSpeed = GetMovementSpeed();
    //    player.characterController.Move((dir * movementSpeed) * Time.deltaTime);
    //}
    //// �̵��ӵ� ��������  
    //private int GetMovementSpeed()
    //{
    //    player.stateMachine.StatHandler.MoveSpeedModifier = 2;  // ���߿� �����丵�� ����

    //    //int moveSpeed = GetStateMachine<PlayerStateMachine>().StatHandler.MoveSpeed * GetStateMachine<PlayerStateMachine>().StatHandler.MoveSpeedModifier;
    //    int moveSpeed = player.stateMachine.StatHandler.MoveSpeed * player.stateMachine.StatHandler.MoveSpeedModifier;
    //    return moveSpeed;
    //}
    #endregion
    //#region ȸ��
    //private void Rotate(Vector3 dir)
    //{
    //    if (dir != Vector3.zero)
    //    {
    //        Transform playerTr = player.transform;
    //        Quaternion targetRotation = Quaternion.LookRotation(dir);
    //        playerTr.rotation = Quaternion.Slerp(playerTr.rotation, targetRotation, player.statHandler.RotationDamping * Time.deltaTime);
    //    }
    //}
    //#endregion

}
