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
public class PlayerController : BaseController
{
    //private PlayerStateMachine stateMachine;
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
    public InputManager inputManager;

    public PlayerInputHandler inputHandler;

    protected CharacterController characterController;
    

    private void Awake()
    {
        // inputManager�� �� �� �����ϴ� ����� �����غ���
        if (inputManager == null)
            inputManager = FindObjectOfType<InputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //BindInputActions();
    }
    /// <summary>
    /// ���� ���⿡���� ��Ʈ��ũ�κ��� �÷��̾��� ������Ʈ�� �޴´�
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Debug.Log("FixedUpdateNetwork ����");
        if (GetInput(out NetworkInputData input))
        {
            // �׽�Ʈ������ �÷��̾ �̵���Ų��

            //// �Է��� ���� ���¿� ����
            //stateMachine.currentState.HandleInput(input);

            //// ���� ������Ʈ
            //stateMachine.currentState.UpdateLogic();
        }
    }

    // ��Ʈ��ũ���� ���� ���¸� �ݿ��Ѵ�
    public void ApplyNetworkState()
    {

    }

    #region ���� ��ȭ ����(PlayerInputHandler�� ���� �����ͼ� �Ǵ�)
    // 1��Ī �ִϸ��̼��� LocalPlayerController 3��Ī �ִϸ��̼��� RemoteController���� ����������
    // ���� ������ �ִϸ��̼��̾�� �ϹǷ� ���� ������ �����Ѵ�
    // �̵�
    public bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    // ����
    public bool IsJumpInput() => inputHandler.GetIsJumping();
    // ���
    public bool IsFiring() => inputHandler.GetIsFiring();

    //���� ĳ���Ͱ� �� ���� �ִ���
    //public bool IsGrounded() => 

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
