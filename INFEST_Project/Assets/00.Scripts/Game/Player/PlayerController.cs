using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

/// <summary>
/// ĳ���� ���� ó���� �Ѵ�
/// ���� �� �ൿ(������, �ִϸ��̼� ��)
/// 
/// 
/// InputAction�� �̺�Ʈ�޼��带 �����Ѵ�
/// 
/// �÷��̾��� FSM�� ��Ʈ��ũ���� ����ȭ�� �Է� �����͸� ������� ���� ��ȯ
/// 
/// �÷��̾��� ���� �� ���� ����
/// ���� ���� �� �÷��̾� ĳ������ ������ �����ϴ� ����.
/// �̵�, ����, ���� �� ������ ������ �����ϸ�, �̸� ��Ʈ��ũ ���¿� �ݿ�.
/// FixedUpdateNetwork()���� Fusion���κ��� ���� �Է� �����͸� ������� �ùķ��̼� ����.
/// 
/// ���� ��� �� ĳ���� ���� ������Ʈ.
/// ��Ʈ��ũ�κ��� ���� ���� ���¸� �ݿ��Ͽ� Ŭ���̾�Ʈ ȭ�鿡 ǥ��.
/// �ִϸ��̼� �� �ð��� ȿ�� ó��.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //BindInputActions();
    }




    // InputAction�� �׿� �´� �̺�Ʈ�� �����Ѵ�

    //private void BindInputActions()
    //{
    //    var actionMap = inputManager;

    //    actionMap.GetInput(EPlayerInput.move).performed += OnMovePerformed;
    //    actionMap.GetInput(EPlayerInput.move).canceled += OnMoveCanceled;

    //    actionMap.GetInput(EPlayerInput.look).performed += OnLookPerformed;
    //    actionMap.GetInput(EPlayerInput.look).canceled += OnLookCanceled;

    //    actionMap.GetInput(EPlayerInput.jump).performed += OnJump;

    //    actionMap.GetInput(EPlayerInput.run).performed += context => IsRunning = true;
    //    actionMap.GetInput(EPlayerInput.run).canceled += context => IsRunning = false;

    //    actionMap.GetInput(EPlayerInput.fire).performed += context => IsFiring = true;
    //    actionMap.GetInput(EPlayerInput.fire).canceled += context => IsFiring = false;

    //    actionMap.GetInput(EPlayerInput.zoom).performed += context => IsZooming = true;
    //    actionMap.GetInput(EPlayerInput.zoom).canceled += context => IsZooming = false;

    //    actionMap.GetInput(EPlayerInput.reload).performed += OnReload;

    //    actionMap.GetInput(EPlayerInput.sit).performed += context => IsSitting = true;
    //    actionMap.GetInput(EPlayerInput.sit).canceled += context => IsSitting = false;

    //    actionMap.GetInput(EPlayerInput.scoreboard).performed += context => IsScoreBoardPopup = true;
    //    actionMap.GetInput(EPlayerInput.scoreboard).canceled += context => IsScoreBoardPopup = false;
    //}

    //// === �� �Է� ó�� �޼��� ===
    //// Move
    //private void OnMovePerformed(InputAction.CallbackContext context)
    //{
    //    Vector2 input = context.ReadValue<Vector2>();
    //    MoveInput = new Vector3(input.x, 0, input.y);
    //}
    //private void OnMoveCanceled(InputAction.CallbackContext context)
    //{
    //    MoveInput = Vector3.zero;
    //}

    //// Look
    //private void OnLookPerformed(InputAction.CallbackContext context)
    //{
    //    Vector2 input = context.ReadValue<Vector2>();
    //    RotationX = input.x;
    //    RotationY = input.y;
    //}
    //private void OnLookCanceled(InputAction.CallbackContext context)
    //{
    //    RotationX = 0f;
    //    RotationY = 0f;
    //}

    //// Jump
    //private void OnJump(InputAction.CallbackContext context)
    //{
    //    IsJumping = true;
    //}

    //// Reload
    //private void OnReload(InputAction.CallbackContext context)
    //{
    //    IsReloading = true;
    //}




}
