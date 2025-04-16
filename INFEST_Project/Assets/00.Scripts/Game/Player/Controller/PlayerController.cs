using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;


/// <summary>
/// ĳ���� ���� ó���� �Ѵ�
/// ���� �� �ൿ(������, �ִϸ��̼� ��)
/// 
/// InputAction�� �̺�Ʈ�޼��带 �����Ѵ�
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
/// 
/// ī�޶� ���⿡�� ������Ʈ(?)
/// 
/// �÷��̾ StateMachine���� ���¸� �ٲٴ� ���� Controller���� 
/// ���� ���¸� ������ ������ �͵� ���⿡�� ���� 
/// 
/// BaseController�� ����� �޴� ������� �ٲپ���
/// </summary>
public abstract class PlayerController : BaseController
{
    //private PlayerStateMachine stateMachine;

    /// <summary>
    /// �÷��̾ �Է��� ������ ���� 2���� ������ ����(2���� ������ ������ �����̴�)
    /// 1.������ ������ �ϴ� PlayerInputHandler�� ����
    /// 2.State ������ ���� �����´� 
    /// ���⼭ �޾ƿͼ� 
    /// ���¸� ������Ʈ �� �� 
    /// NetworkInputData�� ���¸� ���� ����
    /// 
    /// PlayerInputSender���� �������� Ȯ�� �� ������ ������
    /// </summary>
    //public InputManager inputManager;

    /// <summary>
    /// ������� ������ �� �������� �޼������ �÷��̾� �������� �ֻ��� �θ� �پ�� �ϹǷ�
    /// ���߿��� PlayerInputHandler�� �Űܾ� �Ѵ�
    /// ���� 1��Ī �����հ� 3��Ī �����տ� �ٴ� �͵��� animator�� statemachine�� ������ �־���Ѵ�
    /// </summary>
    public PlayerInputHandler inputHandler;

    protected CharacterController controller;

    // FSM ���� �ӽ� �ν��Ͻ�
    protected float verticalVelocity;
    protected float gravity = -9.81f;

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;

    protected override void Awake()
    {
        // inputManager�� �� �� �����ϴ� ����� �����غ���
        //if (inputManager == null)
        //    inputManager = FindObjectOfType<InputManager>();

        controller = GetComponentInParent<CharacterController>();
    }

    // ��Ʈ��ũ���� ���� ���¸� �ݿ��Ѵ�
    public override void ApplyNetworkState(PlayerStatData data)
    {

    }

    #region ���� ��ȭ ����(PlayerInputHandler�� ���� �����ͼ� �Ǵ�)
    // 1��Ī �ִϸ��̼��� LocalPlayerController 3��Ī �ִϸ��̼��� RemoteController���� ����������
    // 1��Ī, 3��Ī �������� ó���ϴ� ���� ���⿡�� ����    
    public override bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    public override bool IsJumpInput() => inputHandler.GetIsJumping();
    public override bool IsFiring() => inputHandler.GetIsFiring();
    public override bool IsGrounded() => controller.isGrounded;
    public override bool IsShotgunFiring() => inputHandler.GetShotgunIsOnFiring();
    public override float GetVerticalVelocity() => verticalVelocity;    
    public override void ApplyGravity() { } 
    #endregion

    // ��Ʈ��ũ���� ���� ���� ���� �ൿ�ϴ� �޼��� �����
    public void OnMove()
    {
        //Vector3 input = _input.MoveInput;

        ///*Vector3 move = head.right * input.x + head.forward * input.z;
        //_controller.Move(move * _statHandler.MoveSpeed * Time.deltaTime);*/

        //Vector3 forward = transform.forward;
        //Vector3 right = transform.right;

        //Vector3 move = right * input.x + forward * input.z;
        //move.y = 0f; // ���� ���� ����
        //_controller.Move(move.normalized * _statHandler.MoveSpeed * Time.deltaTime);
    }


}
