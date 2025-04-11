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
public class PlayerController : NetworkBehaviour
{
    private PlayerStateMachine stateMachine;
    
    public Player player;

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

    private void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }

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
