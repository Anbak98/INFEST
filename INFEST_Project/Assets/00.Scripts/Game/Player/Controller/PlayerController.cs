using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
//using static UnityEditor.Experimental.GraphView.GraphView;


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
    // ���� ����Ǵ� ���� �����
    [HideInInspector]   
    public Player player;


    public Weapons weapons;

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

        weapons = player.GetWeapons();

        //inputHandler = player.Input;
        // inputManager�� �� �� �����ϴ� ����� �����غ���
        //if (inputManager == null)
        //    inputManager = FindObjectOfType<InputManager>();

        //controller = GetComponentInParent<CharacterController>();

        stateMachine = new PlayerStateMachine(player, this);

        //MainCameraTransform = Camera.main.transform;

    }

    //public override void Update()
    //{
    //    // ���¸ӽ� 
    //    stateMachine.HandleInput();
    //    stateMachine.Update();
    //}

    public override void FixedUpdateNetwork()
    {
        //base.FixedUpdateNetwork();


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

    // ���� ���ȳ�
    public override bool IsJumpInput() => player.Input.GetIsJumping();
    public override bool IsSitInput() => player.Input.GetIsSitting();

    // �÷��̾ �� ���� �ִ���?
    public override bool IsGrounded() => player.characterController.isGrounded;
    public override float GetVerticalVelocity() => verticalVelocity;



    // Remote �÷��̾�� ���� �̵����� �ʴ´� (�����κ��� �޴� ������ �ŷ�)

    // �÷��̾��� �̵�(������ CameraHandler���� ����) ó��
    public override void HandleMovement()
    {
        Vector3 input = player.Input.MoveInput;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * input.x + forward * input.z;
        move.y = 0f; // ���� ���� ����
        player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime);
    }
    public override void ApplyGravity()
    {
        // TODO
        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);
        player.characterController.Move(gravityMove * Time.deltaTime);
    }
    /// <summary>
    /// ���� ���� �� ���� �ӵ� ���
    /// </summary>
    public override void StartJump()
    {
        verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);
    }

    // �ɴ´�
    public override void StartSit()
    {
        // collider�� ���¿��� ��ȭ��Ű�Ƿ� ���⼭�� transform�� �Ʒ���
        float playerYpos = player.transform.position.y;
        playerYpos /= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }
    // �Ͼ��
    public override void StartStand()
    {
        // collider�� ���¿��� ��ȭ��Ű�Ƿ� ���⼭�� transform�� �Ʒ���
        float playerYpos = player.transform.position.y;
        playerYpos *= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }


    public override void StartFire()
    {
        if (GetInput(out NetworkInputData data))
        {
            // ��Ʈ��ũ ��ü�� StateAuthority(ȣ��Ʈ)�� ������ �� �ֱ� ������ StateAuthority�� ���� Ȯ���� �ʿ�
            // ȣ��Ʈ������ ����ǰ� Ŭ���̾�Ʈ������ �������� �ʴ´�
            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                // ���콺 ��Ŭ��(����)
                if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
                {
                    //Debug.Log("����");
                    weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                }
            }
        }
    }

    public override void StartReload()
    {
        // TODO
    }



}
