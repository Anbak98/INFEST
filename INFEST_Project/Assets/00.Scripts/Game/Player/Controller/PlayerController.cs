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
/// 
/// InputAction�� �̺�Ʈ�޼��带 �����Ѵ�
/// �÷��̾��� FSM�� ��Ʈ��ũ���� ����ȭ�� �Է� �����͸� ������� ���� ��ȯ
/// 
/// �÷��̾��� ���� �� ���� ����
/// FixedUpdateNetwork()���� Fusion���κ��� ���� �Է� �����͸� ������� �ùķ��̼� ����.
/// </summary>
public abstract class PlayerController : BaseController
{
    public Player player;

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

    /// <summary>
    /// ������� ������ �� �������� �޼������ �÷��̾� �������� �ֻ��� �θ� �پ�� �ϹǷ�
    /// ���߿��� PlayerInputHandler�� �Űܾ� �Ѵ�
    /// ���� 1��Ī �����հ� 3��Ī �����տ� �ٴ� �͵��� animator�� statemachine�� ������ �־���Ѵ�
    /// </summary>
    //public PlayerInputHandler inputHandler;
    public PlayerCameraHandler cameraHandler;
    //public Transform MainCameraTransform { get; set; }

    //protected CharacterController controller;

    // FSM ���� �ӽ� �ν��Ͻ�
    protected float verticalVelocity;
    protected float gravity = -9.81f;

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;

    public override void Awake()
    {        
        player = GetComponentInParent<Player>();    // �÷��̾� ���� ����

        //inputHandler = player.Input;
        // inputManager�� �� �� �����ϴ� ����� �����غ���
        //if (inputManager == null)
        //    inputManager = FindObjectOfType<InputManager>();

        //controller = GetComponentInParent<CharacterController>();

        stateMachine = new PlayerStateMachine(player, this);

        //MainCameraTransform = Camera.main.transform;

    }

    public override void Update()
    {
        // ���¸ӽ� 
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    //#region ���� ��ȭ ����(PlayerInputHandler�� ���� �����ͼ� �Ǵ�)
    //// 1��Ī �ִϸ��̼��� LocalPlayerController 3��Ī �ִϸ��̼��� RemoteController���� ����������
    //// 1��Ī, 3��Ī �������� ó���ϴ� ���� ���⿡�� ����    
    //public override bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    //public override bool IsJumpInput() => inputHandler.GetIsJumping();
    //public override bool IsFiring() => inputHandler.GetIsFiring();
    ////public override bool IsGrounded() => controller.isGrounded;
    //public override bool IsShotgunFiring() => inputHandler.GetShotgunIsOnFiring();
    //public override float GetVerticalVelocity() => verticalVelocity;
    //public override void ApplyGravity() { }
    //#endregion



}
