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
    protected BaseController controller;    // PlayerController, �ٸ� npc�� controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler�� �ִ� �����͸� �б⸸ �Ѵ�
    protected Player player;

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
    }

    public virtual void Enter() 
    {
        AddInputActionsCallbacks(); // �̺�Ʈ ���
    }

    public virtual void Exit() 
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput() 
    {
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
    }
    // �̺�Ʈ ����
    protected virtual void RemoveInputActionsCallbacks()
    {
        // ���¸� �������� �� �߰��Ѵ�
        inputManager.GetInput(EPlayerInput.move).canceled -= OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

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
    #endregion

    #region �ִϸ��̼� ��ü
    // �ִϸ��̼� ��ü
    // bool �ִϸ��̼� ����
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    }
    // Trigger �ִϸ��̼� ����
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
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }
}
